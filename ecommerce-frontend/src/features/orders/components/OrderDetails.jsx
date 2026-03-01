import { useParams, Link } from 'react-router';
import { useOrder } from '../api/orderApi';
import { CheckCircle, ArrowLeft, Calendar, User, Tag } from 'lucide-react';

const OrderDetails = () => {
	const { id } = useParams();
	const { data: order, isLoading, isError } = useOrder(id);

	if (isLoading)
		return <div className="text-center py-20">Loading receipt...</div>;
	if (isError || !order)
		return (
			<div className="text-center py-20 text-red-500">Order not found.</div>
		);

	return (
		<div className="max-w-3xl mx-auto space-y-6 pb-12">
			{/* Back Button */}
			<Link
				to="/products"
				className="flex items-center gap-2 text-gray-500 hover:text-indigo-600 transition-colors w-fit">
				<ArrowLeft size={18} />
				<span>Back to Shopping</span>
			</Link>

			{/* Header Card */}
			<div className="bg-white rounded-2xl shadow-sm border p-8 text-center space-y-4">
				<div className="inline-flex items-center justify-center w-16 h-16 bg-green-100 text-green-600 rounded-full">
					<CheckCircle size={32} />
				</div>
				<h1 className="text-3xl font-bold text-gray-900">Order Confirmed!</h1>
				<p className="text-gray-500">
					Order ID: #ORD-{order.id.toString().padStart(5, '0')}
				</p>
			</div>

			<div className="grid grid-cols-1 md:grid-cols-2 gap-6">
				{/* Customer Info */}
				<div className="bg-white rounded-xl border p-6 flex items-start gap-4">
					<div className="p-3 bg-blue-50 text-blue-600 rounded-lg">
						<User size={20} />
					</div>
					<div>
						<p className="text-sm text-gray-500 uppercase font-semibold">
							Customer
						</p>
						<p className="text-lg font-medium text-gray-900">
							{order.customerName}
						</p>
					</div>
				</div>

				{/* Date Info */}
				<div className="bg-white rounded-xl border p-6 flex items-start gap-4">
					<div className="p-3 bg-purple-50 text-purple-600 rounded-lg">
						<Calendar size={20} />
					</div>
					<div>
						<p className="text-sm text-gray-500 uppercase font-semibold">
							Date
						</p>
						<p className="text-lg font-medium text-gray-900">
							{new Date(order.createdAt).toLocaleDateString()}
						</p>
					</div>
				</div>
			</div>

			{/* Itemized List */}
			<div className="bg-white rounded-xl border overflow-hidden">
				<table className="w-full text-left">
					<thead className="bg-gray-50 border-b">
						<tr>
							<th className="px-6 py-4 text-sm font-semibold text-gray-600">
								Product
							</th>
							<th className="px-6 py-4 text-sm font-semibold text-gray-600 text-center">
								Qty
							</th>
							<th className="px-6 py-4 text-sm font-semibold text-gray-600 text-right">
								Price
							</th>
							<th className="px-6 py-4 text-sm font-semibold text-gray-600 text-right">
								Total
							</th>
						</tr>
					</thead>
					<tbody className="divide-y">
						{order.items.map((item, idx) => (
							<tr key={idx}>
								<td className="px-6 py-4 text-gray-900 font-medium">
									{item.productName}
								</td>
								<td className="px-6 py-4 text-gray-600 text-center">
									{item.quantity}
								</td>
								<td className="px-6 py-4 text-gray-600 text-right">
									${item.unitPrice.toFixed(2)}
								</td>
								<td className="px-6 py-4 text-gray-900 font-semibold text-right">
									${item.lineTotal.toFixed(2)}
								</td>
							</tr>
						))}
					</tbody>
				</table>

				{/* Totals Section */}
				<div className="p-6 bg-gray-50 space-y-3">
					<div className="flex justify-between text-gray-600">
						<span>Subtotal</span>
						<span>${order.subtotal.toFixed(2)}</span>
					</div>

					{order.discountAmount > 0 && (
						<div className="flex justify-between text-green-600 font-medium">
							<span className="flex items-center gap-1">
								<Tag size={16} /> Tiered Discount ({order.discountPercentage}%)
							</span>
							<span>-${order.discountAmount.toFixed(2)}</span>
						</div>
					)}

					<div className="flex justify-between text-xl font-bold text-gray-900 border-t pt-3">
						<span>Grand Total</span>
						<span>${order.totalAmount.toFixed(2)}</span>
					</div>
				</div>
			</div>
		</div>
	);
};

export default OrderDetails;
