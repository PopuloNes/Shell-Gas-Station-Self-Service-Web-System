import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import logoImg from '../assets/logo.png';
import textLogoImg from '../assets/text_logo.jpg';
import api from '../services/api';

const ManagerDashboard = () => {
    const { user, logout } = useAuth();
    const [station, setStation] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [replenishAmount, setReplenishAmount] = useState('');
    const [replenishTankId, setReplenishTankId] = useState(null);

    const [newPumpName, setNewPumpName] = useState('');
    const [transactions, setTransactions] = useState([]);

    const fetchStation = async () => {
        try {
            setLoading(true);
            const [stationRes, txRes] = await Promise.all([
                api.get('/ManagerStation'),
                api.get('/ManagerStation/transactions')
            ]);
            setStation(stationRes.data);
            setTransactions(txRes.data);
            setError(null);
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to load gas station data');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchStation();
    }, []);

    const handleReplenish = async (tankId) => {
        if (!replenishAmount || isNaN(replenishAmount) || Number(replenishAmount) <= 0) {
            alert('Please enter a valid positive amount.');
            return;
        }

        try {
            await api.post(`/ManagerStation/tanks/${tankId}/replenish`, { amount: Number(replenishAmount) });
            alert('Tank replenished successfully!');
            setReplenishTankId(null);
            setReplenishAmount('');
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to replenish tank');
        }
    };

    const handleTogglePumpStatus = async (pumpId, currentStatus) => {
        // Status: 0 = Disabled, 1 = Free, 2 = Maintenance, etc.
        // We toggle between Disabled (0) and Free (1)
        const newStatus = currentStatus === 1 ? 0 : 1; 
        try {
            await api.put(`/ManagerStation/pumps/${pumpId}/status`, { status: newStatus });
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to update pump status');
        }
    };

    const handleAddPump = async () => {
        if (!newPumpName) {
            alert('Please enter a pump name.');
            return;
        }

        try {
            await api.post('/ManagerStation/pumps', { name: newPumpName });
            setNewPumpName('');
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to add pump');
        }
    };

    const handleDeletePump = async (pumpId) => {
        if (!window.confirm('Are you sure you want to delete this pump?')) return;

        try {
            await api.delete(`/ManagerStation/pumps/${pumpId}`);
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to delete pump');
        }
    };

    const handleLinkFuelToPump = async (pumpId, tankId) => {
        try {
            await api.post(`/ManagerStation/pumps/${pumpId}/tanks/${tankId}`);
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to link fuel');
        }
    };

    const handleUnlinkFuelFromPump = async (pumpId, tankId) => {
        try {
            await api.delete(`/ManagerStation/pumps/${pumpId}/tanks/${tankId}`);
            fetchStation();
        } catch (err) {
            alert(err.response?.data?.message || 'Failed to unlink fuel');
        }
    };

    if (loading) return <div className="text-center mt-5 fs-4 text-muted">Loading dashboard...</div>;

    return (
        <div className="container-fluid py-4 bg-light min-vh-100">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
                <div>
                    <h1 className="page-title" style={{ margin: 0 }}>Manager Dashboard</h1>
                    <p style={{ margin: 0, color: '#666' }}>Welcome back, {user?.name}</p>
                </div>
                <button className="btn" onClick={logout} style={{ background: '#ff4d4f', color: '#fff' }}>Logout</button>
            </div>

            {error ? (
                <div className="alert alert-danger shadow-sm border-danger border-2">
                    <h4 className="alert-heading fw-bold">Error</h4>
                    <p className="mb-0">{error}</p>
                </div>
            ) : station ? (
                <>
                    <div className="card shadow-sm border-0 mb-4">
                        <div className="card-body">
                            <h2 className="text-dark fw-bold m-0">{station.name}</h2>
                            <p className="text-muted m-0 mt-1"><i className="bi bi-geo-alt-fill text-danger me-2"></i>{station.address}</p>
                        </div>
                    </div>

                    <div className="row g-4 mb-4">
                        {/* TANKS SECTION */}
                        <div className="col-md-6">
                            <div className="card shadow-sm border-0 h-100">
                                <div className="card-body">
                                    <h4 className="card-title text-danger fw-bold border-bottom pb-2 mb-3">Fuel Tanks Management</h4>
                                    {station.availableFuels?.map(fuel => (
                                        <div key={fuel.tankId} className="border rounded p-3 mb-3 bg-light d-flex justify-content-between align-items-center">
                                            <div>
                                                <h5 className="fw-bold text-dark mb-2">{fuel.fuelName}</h5>
                                                <div className="text-muted small">
                                                    Available Volume: <strong className="text-dark">{fuel.availableVolume.toFixed(2)} L</strong>
                                                </div>
                                                <div className="text-muted small">
                                                    Price: <span className="text-danger fw-bold">{fuel.price.toFixed(2)} PLN/L</span>
                                                </div>
                                            </div>
                                            <div>
                                                {replenishTankId === fuel.tankId ? (
                                                    <div className="d-flex gap-2 align-items-center">
                                                        <input 
                                                            type="number" 
                                                            className="form-control form-control-sm"
                                                            value={replenishAmount} 
                                                            onChange={e => setReplenishAmount(e.target.value)} 
                                                            placeholder="Amount (L)" 
                                                            style={{ width: '100px' }}
                                                        />
                                                        <button className="btn btn-warning btn-sm fw-bold" onClick={() => handleReplenish(fuel.tankId)}>Add</button>
                                                        <button className="btn btn-outline-secondary btn-sm fw-bold" onClick={() => setReplenishTankId(null)}>Cancel</button>
                                                    </div>
                                                ) : (
                                                    <button className="btn btn-outline-danger btn-sm fw-bold" onClick={() => setReplenishTankId(fuel.tankId)}>Replenish</button>
                                                )}
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>

                        {/* PUMPS SECTION */}
                        <div className="col-md-6">
                            <div className="card shadow-sm border-0 h-100">
                                <div className="card-body">
                                    <div className="d-flex justify-content-between align-items-center border-bottom pb-2 mb-3">
                                        <h4 className="card-title text-danger fw-bold m-0">Pumps Management</h4>
                                        <div className="d-flex gap-2">
                                            <input 
                                                type="text" 
                                                className="form-control form-control-sm"
                                                value={newPumpName} 
                                                onChange={e => setNewPumpName(e.target.value)} 
                                                placeholder="New pump name"
                                            />
                                            <button className="btn btn-warning btn-sm fw-bold text-nowrap" onClick={handleAddPump}>Add Pump</button>
                                        </div>
                                    </div>

                                    {station.pumps?.map(pump => (
                                        <div key={pump.id} className="border rounded p-3 mb-3 bg-light">
                                            <div className="d-flex justify-content-between align-items-center mb-3">
                                                <h5 className="fw-bold text-dark m-0">{pump.name}</h5>
                                                <div className="d-flex gap-2 align-items-center">
                                                    <span className={`badge ${pump.status === 1 ? 'bg-success' : 'bg-danger'}`}>
                                                        {pump.status === 1 ? 'Free (Open)' : 'Disabled (Closed)'}
                                                    </span>
                                                    <button 
                                                        className={`btn btn-sm fw-bold ${pump.status === 1 ? 'btn-outline-danger' : 'btn-outline-success'}`}
                                                        onClick={() => handleTogglePumpStatus(pump.id, pump.status)}
                                                    >
                                                        {pump.status === 1 ? 'Close Pump' : 'Open Pump'}
                                                    </button>
                                                    <button 
                                                        className="btn btn-danger btn-sm fw-bold"
                                                        onClick={() => handleDeletePump(pump.id)}
                                                    >
                                                        Delete
                                                    </button>
                                                </div>
                                            </div>

                                            <div>
                                                <p className="small text-muted mb-2 fw-bold">Connected Fuels:</p>
                                                <div className="d-flex flex-wrap gap-2">
                                                    {station.availableFuels?.map(fuel => {
                                                        const isConnected = fuel.pumpIds.includes(pump.id);
                                                        return (
                                                            <div 
                                                                key={fuel.tankId} 
                                                                className={`badge border d-flex align-items-center gap-1 p-2 ${isConnected ? 'bg-success bg-opacity-10 text-success border-success' : 'bg-white text-muted border-secondary'}`}
                                                            >
                                                                {fuel.fuelName}
                                                                {isConnected ? (
                                                                    <button 
                                                                        className="btn-close btn-close-white"
                                                                        style={{ width: '0.5em', height: '0.5em', filter: 'invert(1) grayscale(100%) brightness(0.5)' }}
                                                                        onClick={() => handleUnlinkFuelFromPump(pump.id, fuel.tankId)}
                                                                        title="Unlink"
                                                                    ></button>
                                                                ) : (
                                                                    <button 
                                                                        className="btn btn-link p-0 m-0 text-success text-decoration-none"
                                                                        style={{ lineHeight: '1', fontSize: '14px' }}
                                                                        onClick={() => handleLinkFuelToPump(pump.id, fuel.tankId)}
                                                                        title="Link"
                                                                    >
                                                                        ➕
                                                                    </button>
                                                                )}
                                                            </div>
                                                        );
                                                    })}
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                    {station.pumps?.length === 0 && (
                                        <p className="text-muted text-center mt-3">No pumps available.</p>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="card shadow-sm border-0">
                        <div className="card-body">
                            <h4 className="card-title text-danger fw-bold border-bottom pb-2 mb-3">Recent Transactions</h4>
                        {transactions.length > 0 ? (
                            <div className="table-responsive">
                                <table className="table table-hover table-bordered border-light align-middle">
                                    <thead className="table-light text-muted small">
                                        <tr>
                                            <th>Date</th>
                                            <th>Pump</th>
                                            <th>Fuel</th>
                                            <th>Liters</th>
                                            <th>Amount (PLN)</th>
                                            <th>Client Phone</th>
                                            <th>Bonus Card</th>
                                            <th>Payment Card</th>
                                        </tr>
                                    </thead>
                                    <tbody className="bg-white">
                                        {transactions.map(tx => (
                                            <tr key={tx.orderId}>
                                                <td className="text-dark">{new Date(tx.date).toLocaleString()}</td>
                                                <td className="text-dark fw-bold">{tx.pumpName}</td>
                                                <td className="text-dark">{tx.fuelName}</td>
                                                <td className="text-dark">{tx.volume.toFixed(2)} L</td>
                                                <td className="text-success fw-bold">{tx.amount.toFixed(2)} PLN</td>
                                                <td className="text-dark">{tx.clientPhone}</td>
                                                <td className="text-dark">{tx.clientBarcode}</td>
                                                <td className="text-dark">{tx.paymentCard}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        ) : (
                            <p className="text-muted text-center mt-3 mb-0">No transactions found.</p>
                        )}
                        </div>
                    </div>
                </>
            ) : null}
        </div>
    );
};

export default ManagerDashboard;
