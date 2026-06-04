import React, { useState, useEffect } from 'react';
import adminService from '../../services/adminService';

const GlobalStatistics = () => {
    const [transactions, setTransactions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [gasStations, setGasStations] = useState([]);
    const [filterStationId, setFilterStationId] = useState('');

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        fetchTransactions();
    }, [filterStationId]);

    const fetchData = async () => {
        try {
            const stations = await adminService.getGasStations();
            setGasStations(stations);
            await fetchTransactions();
        } catch (error) {
            console.error('Failed to fetch initial data', error);
        }
    };

    const fetchTransactions = async () => {
        try {
            setLoading(true);
            const data = await adminService.getTransactions(filterStationId);
            setTransactions(data);
        } catch (error) {
            console.error('Failed to fetch transactions', error);
        } finally {
            setLoading(false);
        }
    };

    const totalRevenue = transactions.reduce((sum, t) => sum + t.amount, 0);
    const totalVolume = transactions.reduce((sum, t) => sum + t.volume, 0);

    return (
        <div className="admin-section">
            <h2>Global Statistics</h2>
            
            <div className="stats-cards">
                <div className="stat-card">
                    <h4>Total Transactions</h4>
                    <p>{transactions.length}</p>
                </div>
                <div className="stat-card">
                    <h4>Total Revenue</h4>
                    <p>${totalRevenue.toFixed(2)}</p>
                </div>
                <div className="stat-card">
                    <h4>Total Volume</h4>
                    <p>{totalVolume.toFixed(2)} L</p>
                </div>
            </div>

            <div className="filter-controls" style={{ margin: '20px 0' }}>
                <label>Filter by Station: </label>
                <select value={filterStationId} onChange={(e) => setFilterStationId(e.target.value)}>
                    <option value="">All Stations</option>
                    {gasStations.map(s => (
                        <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                </select>
            </div>

            <div className="table-container">
                {loading ? (
                    <div>Loading...</div>
                ) : (
                    <table>
                        <thead>
                            <tr>
                                <th>Order ID</th>
                                <th>Date</th>
                                <th>Pump</th>
                                <th>Fuel</th>
                                <th>Volume</th>
                                <th>Amount</th>
                                <th>Client Phone</th>
                            </tr>
                        </thead>
                        <tbody>
                            {transactions.map(t => (
                                <tr key={t.orderId}>
                                    <td>{t.orderId}</td>
                                    <td>{new Date(t.date).toLocaleString()}</td>
                                    <td>{t.pumpName}</td>
                                    <td>{t.fuelName}</td>
                                    <td>{t.volume.toFixed(2)}</td>
                                    <td>${t.amount.toFixed(2)}</td>
                                    <td>{t.clientPhone}</td>
                                </tr>
                            ))}
                            {transactions.length === 0 && (
                                <tr>
                                    <td colSpan="7" style={{ textAlign: 'center' }}>No transactions found.</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                )}
            </div>
        </div>
    );
};

export default GlobalStatistics;
