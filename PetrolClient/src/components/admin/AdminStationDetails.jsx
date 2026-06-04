import React, { useState, useEffect } from 'react';
import adminService from '../../services/adminService';

const AdminStationDetails = ({ stationId, onBack }) => {
    const [station, setStation] = useState(null);
    const [loading, setLoading] = useState(true);

    const fetchDetails = async () => {
        try {
            setLoading(true);
            const data = await adminService.getGasStationDetails(stationId);
            setStation(data);
        } catch (error) {
            console.error('Failed to fetch station details', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (stationId) {
            fetchDetails();
        }
    }, [stationId]);

    const handlePriceChange = async (fuelId, newPrice) => {
        const parsedPrice = parseFloat(newPrice);
        if (isNaN(parsedPrice) || parsedPrice < 0) {
            alert('Invalid price');
            return;
        }
        try {
            await adminService.updateFuelPrice(fuelId, parsedPrice);
            fetchDetails(); // Refresh
        } catch (error) {
            console.error('Failed to update price', error);
            alert('Failed to update price');
        }
    };

    if (loading) return <div>Loading Station Details...</div>;
    if (!station) return <div>Station not found.</div>;

    return (
        <div className="admin-section">
            <button onClick={onBack} style={{marginBottom: '20px', background: 'transparent', color: 'var(--primary-color)', border: '1px solid var(--primary-color)', padding: '8px 16px', borderRadius: '5px', cursor: 'pointer'}}>
                &larr; Back to Gas Stations
            </button>
            <h2>{station.name}</h2>
            <p style={{opacity: 0.7}}>{station.address}</p>

            <div className="row mt-4">
                {/* Pumps */}
                <div className="col-md-6 mb-4">
                    <div className="card shadow-sm border-0 h-100">
                        <div className="card-body">
                            <h3 className="card-title text-danger fw-bold border-bottom pb-2 mb-3">Pumps Status</h3>
                            <div className="table-responsive">
                                <table className="table table-hover align-middle">
                                    <thead className="table-light">
                                        <tr>
                                            <th>Name</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {station.pumps?.map(p => (
                                            <tr key={p.id}>
                                                <td className="fw-bold">{p.name}</td>
                                                <td>
                                                    <span className={`badge ${
                                                        p.status === 2 ? 'bg-success' : 
                                                        p.status === 3 ? 'bg-warning text-dark' : 
                                                        'bg-danger'
                                                    } fs-6`}>
                                                        {p.status === 0 ? 'Disabled' : p.status === 1 ? 'Maintenance' : p.status === 2 ? 'Free' : 'Busy'}
                                                    </span>
                                                </td>
                                            </tr>
                                        ))}
                                        {!station.pumps?.length && <tr><td colSpan="2" className="text-center text-muted py-3">No pumps available.</td></tr>}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Fuels */}
                <div className="col-md-6 mb-4">
                    <div className="card shadow-sm border-0 h-100">
                        <div className="card-body">
                            <h3 className="card-title text-danger fw-bold border-bottom pb-2 mb-3">Available Fuels</h3>
                            <div className="table-responsive">
                                <table className="table table-hover align-middle">
                                    <thead className="table-light">
                                        <tr>
                                            <th>Fuel Type</th>
                                            <th>Volume (L)</th>
                                            <th>Price</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {station.availableFuels?.map(f => (
                                            <tr key={f.tankId}>
                                                <td className="fw-bold">{f.fuelName}</td>
                                                <td>{f.availableVolume.toFixed(2)}</td>
                                                <td className="fw-bold text-success">${f.price.toFixed(2)}</td>
                                                <td>
                                                    <button 
                                                        className="btn btn-outline-warning btn-sm fw-bold text-dark"
                                                        onClick={() => {
                                                            const newPrice = prompt(`Enter new price for ${f.fuelName}:`, f.price);
                                                            if (newPrice !== null) {
                                                                handlePriceChange(f.fuelTypeId, newPrice);
                                                            }
                                                        }}
                                                    >
                                                        Adjust Price
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                        {!station.availableFuels?.length && <tr><td colSpan="4" className="text-center text-muted py-3">No fuels available.</td></tr>}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdminStationDetails;
