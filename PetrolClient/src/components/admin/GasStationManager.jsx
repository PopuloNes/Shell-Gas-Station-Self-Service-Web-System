import React, { useState, useEffect } from 'react';
import adminService from '../../services/adminService';

const GasStationManager = ({ onInspect }) => {
    const [stations, setStations] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editingStation, setEditingStation] = useState(null);
    const [formData, setFormData] = useState({ name: '', address: '', latitude: '', longitude: '' });

    useEffect(() => {
        fetchStations();
    }, []);

    const fetchStations = async () => {
        try {
            setLoading(true);
            const data = await adminService.getGasStations();
            setStations(data);
        } catch (error) {
            console.error('Failed to fetch gas stations', error);
        } finally {
            setLoading(false);
        }
    };

    const handleInputChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const payload = {
                ...formData,
                latitude: parseFloat(formData.latitude),
                longitude: parseFloat(formData.longitude)
            };

            if (editingStation) {
                await adminService.updateGasStation(editingStation.id, payload);
            } else {
                await adminService.createGasStation(payload);
            }
            setFormData({ name: '', address: '', latitude: '', longitude: '' });
            setEditingStation(null);
            fetchStations();
        } catch (error) {
            console.error('Failed to save gas station', error);
            alert('Error saving gas station');
        }
    };

    const handleEdit = (station) => {
        setEditingStation(station);
        setFormData({
            name: station.name,
            address: station.address,
            latitude: station.latitude.toString(),
            longitude: station.longitude.toString()
        });
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Are you sure you want to delete this gas station?')) return;
        try {
            await adminService.deleteGasStation(id);
            fetchStations();
        } catch (error) {
            console.error('Failed to delete gas station', error);
            alert('Error deleting gas station');
        }
    };

    const handleToggleAll = async (status) => {
        if (!window.confirm(`Are you sure you want to turn ${status === 2 ? 'ON' : 'OFF'} all pumps across all stations?`)) return;
        try {
            await adminService.toggleAllPumps(status);
            alert(`All pumps turned ${status === 2 ? 'ON' : 'OFF'} successfully.`);
        } catch (error) {
            console.error('Failed to toggle all pumps', error);
            alert('Error toggling pumps');
        }
    };

    if (loading) return <div>Loading Gas Stations...</div>;

    return (
        <div className="admin-section">
            <div style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                <h2>Gas Stations Management</h2>
                <div>
                    <button className="btn btn-success fw-bold text-white me-2" onClick={() => handleToggleAll(2)}>Turn On All Pumps</button>
                    <button className="btn btn-danger fw-bold text-white" onClick={() => handleToggleAll(0)}>Turn Off All Pumps</button>
                </div>
            </div>
            <div className="form-container">
                <h3>{editingStation ? 'Edit Gas Station' : 'Add New Gas Station'}</h3>
                <form onSubmit={handleSubmit}>
                    <input type="text" name="name" value={formData.name} onChange={handleInputChange} placeholder="Name" required />
                    <input type="text" name="address" value={formData.address} onChange={handleInputChange} placeholder="Address" required />
                    <input type="number" step="any" name="latitude" value={formData.latitude} onChange={handleInputChange} placeholder="Latitude" required />
                    <input type="number" step="any" name="longitude" value={formData.longitude} onChange={handleInputChange} placeholder="Longitude" required />
                    <button type="submit">{editingStation ? 'Update' : 'Create'}</button>
                    {editingStation && <button type="button" onClick={() => { setEditingStation(null); setFormData({ name: '', address: '', latitude: '', longitude: '' }); }}>Cancel</button>}
                </form>
            </div>

            <div className="table-container">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Address</th>
                            <th>Managers</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {stations.map(s => (
                            <tr key={s.id}>
                                <td>{s.id}</td>
                                <td>{s.name}</td>
                                <td>{s.address}</td>
                                <td>
                                    {s.managers && s.managers.length > 0 ? (
                                        s.managers.map(m => <div key={m.id}>{m.fullName} ({m.username})</div>)
                                    ) : (
                                        <span style={{color: '#999'}}>No managers</span>
                                    )}
                                </td>
                                <td>
                                    <button onClick={() => onInspect(s.id)} style={{borderColor: 'var(--secondary-color)', color: 'var(--secondary-color)'}}>Inspect</button>
                                    <button onClick={() => handleEdit(s)}>Edit</button>
                                    <button onClick={() => handleDelete(s.id)} className="delete-btn">Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default GasStationManager;
