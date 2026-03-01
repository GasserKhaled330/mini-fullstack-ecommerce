import { useMutation, useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router';
import api from '../../../api/axios';
import toast from 'react-hot-toast';

export const useCreateOrder = () => {
	const navigate = useNavigate();

	return useMutation({
		mutationFn: (orderData) => api.post('/orders', orderData),
		onSuccess: (response) => {
			toast.success('Order placed successfully!');

			navigate(`/orders/${response.data.id}`);
		},
	});
};

export const useOrder = (id) => {
	return useQuery({
		queryKey: ['orders', id],
		queryFn: async () => {
			const { data } = await api.get(`/orders/${id}`);
			return data;
		},
		enabled: !!id,
	});
};
