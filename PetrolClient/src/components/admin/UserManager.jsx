import React, { useState, useEffect } from 'react';
import adminService from '../../services/adminService';

const UserManager = () => {
    const [users, setUsers] = useState([]);
    const [gasStations, setGasStations] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editingUser, setEditingUser] = useState(null);
    const [formData, setFormData] = useState({
        fullName: '',
        username: '',
        password: '',
        role: 'Client',
        phoneNumber: '',
        email: '',
        gasStationId: ''
    });

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            setLoading(true);
            const [usersData, stationsData] = await Promise.all([
                adminService.getUsers(),
                adminService.getGasStations()
            ]);
            setUsers(usersData);
            setGasStations(stationsData);
        } catch (error) {
            console.error('Failed to fetch data', error);
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
                gasStationId: formData.role === 'Manager' && formData.gasStationId ? parseInt(formData.gasStationId) : null
            };

            if (editingUser) {
                await adminService.updateUser(editingUser.id, payload);
            } else {
                if (!payload.password) {
                    alert('Password is required for new users.');
                    return;
                }
                await adminService.createUser(payload);
            }
            
            resetForm();
            fetchData();
        } catch (error) {
            console.error('Failed to save user', error);
            alert(error.response?.data?.message || 'Error saving user');
        }
    };

    const handleEdit = (user) => {
        setEditingUser(user);
        setFormData({
            fullName: user.fullName || '',
            username: user.username || '',
            password: '', 
            role: user.role || 'Client',
            phoneNumber: user.phoneNumber || '',
            email: user.email || '',
            gasStationId: user.gasStationId || ''
        });
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Are you sure you want to delete this user?')) return;
        try {
            await adminService.deleteUser(id);
            fetchData();
        } catch (error) {
            console.error('Failed to delete user', error);
            alert(error.response?.data?.message || 'Error deleting user');
        }
    };

    const resetForm = () => {
        setEditingUser(null);
        setFormData({
            fullName: '',
            username: '',
            password: '',
            role: 'Client',
            phoneNumber: '',
            email: '',
            gasStationId: ''
        });
    };

    if (loading) return <div>Loading Users...</div>;

    return (
        <div className="admin-section">
            <h2>Users Management</h2>
            <div className="form-container">
                <h3>{editingUser ? 'Edit User' : 'Add New User'}</h3>
                <form onSubmit={handleSubmit} className="user-form">
                    <input type="text" name="fullName" value={formData.fullName} onChange={handleInputChange} placeholder="Full Name" required />
                    <input type="text" name="username" value={formData.username} onChange={handleInputChange} placeholder="Username" required />
                    
                    <input 
                        type="password" 
                        name="password" 
                        value={formData.password} 
                        onChange={handleInputChange} 
                        placeholder={editingUser ? "New Password (leave blank to keep)" : "Password"} 
                        required={!editingUser} 
                    />

                    <select name="role" value={formData.role} onChange={handleInputChange} required>
                        <option value="Client">Client</option>
                        <option value="Manager">Manager</option>
                        <option value="Admin">Admin</option>
                        <option value="Cashier">Cashier</option>
                    </select>

                    <input type="tel" name="phoneNumber" value={formData.phoneNumber} onChange={handleInputChange} placeholder="Phone Number" />
                    <input type="email" name="email" value={formData.email} onChange={handleInputChange} placeholder="Email" />

                    {formData.role === 'Manager' && (
                        <select name="gasStationId" value={formData.gasStationId} onChange={handleInputChange} required>
                            <option value="">Select Gas Station</option>
                            {gasStations.map(s => (
                                <option key={s.id} value={s.id}>{s.name} - {s.address}</option>
                            ))}
                        </select>
                    )}

                    <button type="submit">{editingUser ? 'Update' : 'Create'}</button>
                    {editingUser && <button type="button" onClick={resetForm}>Cancel</button>}
                </form>
            </div>

            <div className="table-container">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Username</th>
                            <th>Role</th>
                            <th>Phone</th>
                            <th>Bonus Card</th>
                            <th>Balance</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map(u => (
                            <tr key={u.id}>
                                <td>{u.id}</td>
                                <td>{u.fullName}</td>
                                <td>{u.username}</td>
                                <td>{u.role}</td>
                                <td>{u.phoneNumber || '-'}</td>
                                <td>{u.bonusCardBarcode || '-'}</td>
                                <td>{u.bonusBalance !== null ? u.bonusBalance : '-'}</td>
                                <td>
                                    <button onClick={() => handleEdit(u)}>Edit</button>
                                    <button onClick={() => handleDelete(u.id)} className="delete-btn">Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default UserManager;
