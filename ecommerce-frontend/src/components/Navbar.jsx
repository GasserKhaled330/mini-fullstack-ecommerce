import React from 'react';
import { NavLink } from 'react-router';
import { ShoppingCart, Package, ShoppingBag } from 'lucide-react';

const Navbar = () => {
	const navItems = [
		{ path: '/products', name: 'Products', icon: <Package size={20} /> },
		{
			path: '/orders/new',
			name: 'Create Order',
			icon: <ShoppingCart size={20} />,
		},
	];

	return (
		<nav className="bg-white border-b border-gray-200 sticky top-0 z-50">
			<div className="container mx-auto px-4">
				<div className="flex justify-between items-center h-16">
					{/* Logo / Brand */}
					<NavLink
						to="/"
						className="flex items-center gap-2 font-bold text-xl text-indigo-600">
						<ShoppingBag className="text-indigo-600" />
						<span>E-Store</span>
					</NavLink>

					{/* Links */}
					<div className="flex gap-6">
						{navItems.map((item) => (
							<NavLink
								key={item.path}
								to={item.path}
								className={({ isActive }) =>
									`flex items-center gap-2 text-sm font-medium transition-colors ${
										isActive
											? 'text-indigo-600 border-b-2 border-indigo-600 pb-5 mt-5'
											: 'text-gray-500 hover:text-indigo-600'
									}`
								}>
								{item.icon}
								{item.name}
							</NavLink>
						))}
					</div>
				</div>
			</div>
		</nav>
	);
};

export default Navbar;
