import React, { useState, useEffect } from 'react';
import adminService from '../../services/adminService';

const FuelTypeManager = () => {
    const [fuelTypes, setFuelTypes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [newFuelName, setNewFuelName] = useState('');
    const [newFuelPrice, setNewFuelPrice] = useState('');

    const fetchFuelTypes = async () => {
        try {
            setLoading(true);
            const data = await adminService.getFuelTypes();
            setFuelTypes(data);
        } catch (error) {
            console.error('Failed to load fuel types', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchFuelTypes();
    }, []);

    const handleCreateFuelType = async (e) => {
        e.preventDefault();
        const price = parseFloat(newFuelPrice);
        if (!newFuelName || isNaN(price) || price < 0) {
            alert('Please enter valid name and price.');
            return;
        }

        try {
            await adminService.createFuelType(newFuelName, price);
            setNewFuelName('');
            setNewFuelPrice('');
            fetchFuelTypes();
        } catch (error) {
            console.error('Failed to create fuel type', error);
            alert(error.response?.data?.message || 'Failed to create fuel type');
        }
    };

    const handleUpdatePrice = async (id, currentPrice, name) => {
        const newPriceStr = prompt(`Enter new price for ${name}:`, currentPrice);
        if (newPriceStr !== null) {
            const newPrice = parseFloat(newPriceStr);
            if (isNaN(newPrice) || newPrice < 0) {
                alert('Invalid price');
                return;
            }
            try {
                await adminService.updateFuelPrice(id, newPrice);
                fetchFuelTypes();
            } catch (error) {
                console.error('Failed to update price', error);
                alert('Failed to update price');
            }
        }
    };

    const handleDelete = async (id, name) => {
        if (window.confirm(`Are you sure you want to delete ${name}?`)) {
            try {
                await adminService.deleteFuelType(id);
                fetchFuelTypes();
            } catch (error) {
                console.error('Failed to delete fuel type', error);
                alert('Failed to delete fuel type');
            }
        }
    };

    if (loading) return <div>Loading Fuel Types...</div>;

    return (
        <div className="admin-section">
            <div className="d-flex justify-content-between align-items-center mb-4 pb-2 border-bottom border-danger">
                <h2 className="text-danger fw-bold m-0">Global Fuel Types</h2>
            </div>

            <div className="card shadow-sm border-0 mb-4">
                <div className="card-body">
                    <h5 className="card-title fw-bold text-dark mb-3">Add New Fuel Type</h5>
                    <form onSubmit={handleCreateFuelType} className="d-flex gap-3 align-items-end">
                        <div className="flex-grow-1">
                            <label className="form-label text-muted small fw-bold">Name (e.g. A-95)</label>
                            <input 
                                type="text" 
                                className="form-control" 
                                value={newFuelName}
                                onChange={e => setNewFuelName(e.target.value)}
                                required
                            />
                        </div>
                        <div className="flex-grow-1">
                            <label className="form-label text-muted small fw-bold">Price (PLN/L)</label>
                            <input 
                                type="number" 
                                step="0.01" 
                                className="form-control" 
                                value={newFuelPrice}
                                onChange={e => setNewFuelPrice(e.target.value)}
                                required
                            />
                        </div>
                        <button type="submit" className="btn btn-warning fw-bold text-dark">
                            Add Fuel Type
                        </button>
                    </form>
                </div>
            </div>

            <div className="card shadow-sm border-0">
                <div className="card-body p-0">
                    <div className="table-responsive">
                        <table className="table table-hover align-middle mb-0">
                            <thead className="table-light">
                                <tr>
                                    <th className="px-4">Fuel Type</th>
                                    <th>Price (PLN/L)</th>
                                    <th className="px-4 text-end">Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {fuelTypes.map(f => (
                                    <tr key={f.id}>
                                        <td className="px-4 fw-bold">{f.name}</td>
                                        <td className="fw-bold text-success">${f.price.toFixed(2)}</td>
                                        <td className="px-4 text-end">
                                            <button 
                                                className="btn btn-outline-warning btn-sm fw-bold text-dark me-2"
                                                onClick={() => handleUpdatePrice(f.id, f.price, f.name)}
                                            >
                                                Adjust Price
                                            </button>
                                            <button 
                                                className="btn btn-danger btn-sm fw-bold"
                                                onClick={() => handleDelete(f.id, f.name)}
                                            >
                                                Delete
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                                {fuelTypes.length === 0 && (
                                    <tr>
                                        <td colSpan="3" className="text-center py-4 text-muted">No fuel types configured.</td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default FuelTypeManager;
