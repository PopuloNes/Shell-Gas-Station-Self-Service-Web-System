import api from './api';

const adminService = {
    // --- GAS STATIONS ---
    getGasStations: async () => {
        const response = await api.get('/admin/gasstations');
        return response.data;
    },
    createGasStation: async (data) => {
        const response = await api.post('/admin/gasstations', data);
        return response.data;
    },
    updateGasStation: async (id, data) => {
        const response = await api.put(`/admin/gasstations/${id}`, data);
        return response.data;
    },
    deleteGasStation: async (id) => {
        const response = await api.delete(`/admin/gasstations/${id}`);
        return response.data;
    },
    getGasStationDetails: async (id) => {
        const response = await api.get(`/admin/gasstations/${id}/details`);
        return response.data;
    },
    toggleAllPumps: async (status) => {
        const response = await api.post('/admin/gasstations/toggle-pumps', { status });
        return response.data;
    },
    updateFuelPrice: async (id, price) => {
        const response = await api.put(`/admin/fueltypes/${id}/price`, { price });
        return response.data;
    },

    // --- USERS ---
    getUsers: async () => {
        const response = await api.get('/admin/users');
        return response.data;
    },
    createUser: async (data) => {
        const response = await api.post('/admin/users', data);
        return response.data;
    },
    updateUser: async (id, data) => {
        const response = await api.put(`/admin/users/${id}`, data);
        return response.data;
    },
    deleteUser: async (id) => {
        const response = await api.delete(`/admin/users/${id}`);
        return response.data;
    },

    // --- STATISTICS ---
    getTransactions: async (stationId = null) => {
        let url = '/admin/transactions';
        if (stationId) {
            url += `?stationId=${stationId}`;
        }
        const response = await api.get(url);
        return response.data;
    }
};

export default adminService;
