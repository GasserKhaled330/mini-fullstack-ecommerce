import { BrowserRouter, Routes, Route, Navigate } from 'react-router';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Toaster } from 'react-hot-toast';
import Navbar from './components/Navbar.jsx';
import ProductList from './features/products/components/ProductList.jsx';
import OrderCreate from './features/orders/components/OrderCreate.jsx';
import OrderDetails from './features/orders/components/OrderDetails.jsx';

const queryClient = new QueryClient();

function App() {
	return (
		<QueryClientProvider client={queryClient}>
			<BrowserRouter>
				<div className="min-h-screen bg-gray-50">
					<Navbar />
					<main className="container mx-auto p-4">
						<Routes>
							<Route path="/" element={<Navigate to="/products" />} />
							<Route path="/products" element={<ProductList />} />
							<Route path="/orders/new" element={<OrderCreate />} />
							<Route path="/orders/:id" element={<OrderDetails />} />
						</Routes>
					</main>
				</div>
				<Toaster position="top-right" />
			</BrowserRouter>
		</QueryClientProvider>
	);
}

export default App;
