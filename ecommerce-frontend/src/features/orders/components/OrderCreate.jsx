import { useState } from 'react';
import { useProducts } from '../../products/api/productApi';
import { useCreateOrder } from '../api/orderApi';
import { Trash2, Plus, Minus, ShoppingCart } from 'lucide-react';

const OrderCreate = () => {
	const { data: productData, isLoading } = useProducts(1, 100);
	const createOrderMutation = useCreateOrder();

	const [customerName, setCustomerName] = useState('');
	const [selectedItems, setSelectedItems] = useState([]);

	const subtotal = selectedItems.reduce(
		(sum, item) => sum + item.price * item.quantity,
		0,
	);
	const totalQuantity = selectedItems.reduce(
		(sum, item) => sum + item.quantity,
		0,
	);

	let discountPct = totalQuantity >= 5 ? 0.1 : totalQuantity >= 2 ? 0.05 : 0;

	const discountAmount = subtotal * discountPct;
	const finalTotal = subtotal - discountAmount;

	const handleAddItem = (product) => {
		if (product.quantity <= 0) return;

		setSelectedItems((prev) => {
			const existing = prev.find((i) => i.productId === product.id);
			if (existing) return prev;
			// if (existing) {
			//     return prev.map(i => i.productId === product.id
			//         ? { ...i, quantity: i.quantity + 1 } : i);
			// }
			return [
				...prev,
				{
					productId: product.id,
					name: product.name,
					price: product.price,
					quantity: 1,
					maxStock: product.quantity,
				},
			];
		});
	};

	const handleUpdateQuantity = (productId, newQty) => {
		if (newQty < 1) return;
		setSelectedItems((prev) =>
			prev.map((item) =>
				item.productId === productId ? { ...item, quantity: newQty } : item,
			),
		);
	};

	const handleRemoveItem = (productId) => {
		setSelectedItems((prev) =>
			prev.filter((item) => item.productId !== productId),
		);
	};

	const handleConfirmOrder = (e) => {
		const payload = {
			customerName,
			items: selectedItems.map(({ productId, quantity }) => ({
				productId,
				quantity,
			})),
		};
		createOrderMutation.mutate(payload);
	};

	if (isLoading) return <p>Loading products...</p>;

	return (
		<div className="grid grid-cols-1 lg:grid-cols-3 gap-8 p-4">
			{/* Left: Product Selection */}
			<div className="lg:col-span-2">
				<h2 className="text-2xl font-bold mb-6 flex items-center gap-2">
					<ShoppingCart className="text-indigo-600" /> Available Products
				</h2>
				<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
					{productData?.items.map((product) => {
						const isAdded = selectedItems.some(
							(i) => i.productId === product.id,
						);
						return (
							<div
								key={product.id}
								className="bg-white p-5 rounded-xl border border-gray-100 shadow-sm flex flex-col justify-between">
								<div>
									<h3 className="font-bold text-gray-800">{product.name}</h3>
									<p className="text-sm text-gray-500 mb-2">
										{product.description}
									</p>
									<p className="text-xl font-bold text-indigo-600">
										${product.price}
									</p>
									<p className="text-xs text-gray-400 mt-1">
										Stock: {product.quantity}
									</p>
								</div>

								<button
									onClick={() => handleAddItem(product)}
									disabled={product.quantity <= 0 || isAdded}
									className={`mt-4 w-full py-2 rounded-lg font-semibold transition-all ${
										isAdded
											? 'bg-green-100 text-green-700 cursor-default'
											: 'bg-indigo-600 text-white hover:bg-indigo-700 disabled:bg-gray-200'
									}`}>
									{isAdded
										? 'Added to Order'
										: product.quantity <= 0
											? 'Out of Stock'
											: 'Add to Order'}
								</button>
							</div>
						);
					})}
				</div>
			</div>

			{/* Right: Order Summary */}
			<div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100 h-fit sticky top-24">
				<h2 className="text-xl font-bold mb-4">Order Summary</h2>

				<div className="space-y-4 mb-6">
					<label className="block text-sm font-medium text-gray-700">
						Customer Name
					</label>
					<input
						type="text"
						className="w-full p-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-indigo-500 outline-none"
						placeholder="Enter customer name..."
						value={customerName}
						onChange={(e) => setCustomerName(e.target.value)}
					/>
				</div>

				<div className="space-y-3 mb-6 max-h-80 overflow-y-auto pr-2">
					{selectedItems.length === 0 && (
						<p className="text-center text-gray-400 py-4 italic">
							No items added yet.
						</p>
					)}
					{selectedItems.map((item) => (
						<div
							key={item.productId}
							className="flex flex-col p-3 bg-gray-50 rounded-lg group">
							<div className="flex justify-between items-start">
								<span className="font-medium text-gray-800">{item.name}</span>
								<button
									onClick={() => handleRemoveItem(item.productId)}
									className="text-gray-400 hover:text-red-500 transition-colors">
									<Trash2 size={18} />
								</button>
							</div>

							<div className="flex justify-between items-center mt-3">
								<div className="flex items-center gap-2 bg-white border rounded-md p-1">
									<button
										onClick={() =>
											handleUpdateQuantity(item.productId, item.quantity - 1)
										}
										className="p-1 hover:text-indigo-600">
										<Minus size={14} />
									</button>
									<input
										type="number"
										value={item.quantity}
										onChange={(e) =>
											handleUpdateQuantity(
												item.productId,
												parseInt(e.target.value) || 1,
											)
										}
										className="w-10 text-center text-sm font-bold bg-transparent outline-none"
									/>
									<button
										onClick={() =>
											handleUpdateQuantity(item.productId, item.quantity + 1)
										}
										className="p-1 hover:text-indigo-600">
										<Plus size={14} />
									</button>
								</div>
								<span className="font-semibold text-gray-700">
									${(item.price * item.quantity).toFixed(2)}
								</span>
							</div>
						</div>
					))}
				</div>

				{/* 3. Totals Section */}
				<div className="border-t pt-4 space-y-2">
					<div className="flex justify-between text-gray-600">
						<span>Subtotal</span>
						<span>${subtotal.toFixed(2)}</span>
					</div>
					{discountPct > 0 && (
						<div className="flex justify-between text-green-600 font-medium">
							<span>Discount ({discountPct * 100}%)</span>
							<span>-${discountAmount.toFixed(2)}</span>
						</div>
					)}
					<div className="flex justify-between text-xl font-bold text-gray-900 border-t pt-2">
						<span>Total</span>
						<span>${finalTotal.toFixed(2)}</span>
					</div>
				</div>

				<button
					onClick={handleConfirmOrder}
					disabled={
						selectedItems.length === 0 ||
						!customerName ||
						createOrderMutation.isPending
					}
					className="mt-6 w-full bg-indigo-600 text-white py-4 rounded-xl font-bold hover:bg-indigo-700 disabled:bg-gray-200 transition-all shadow-md active:scale-95">
					{createOrderMutation.isPending ? 'Placing Order...' : 'Confirm Order'}
				</button>
			</div>
		</div>
	);
};

export default OrderCreate;
