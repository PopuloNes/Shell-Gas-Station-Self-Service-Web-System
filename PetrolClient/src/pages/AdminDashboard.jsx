import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import GasStationManager from '../components/admin/GasStationManager';
import UserManager from '../components/admin/UserManager';
import GlobalStatistics from '../components/admin/GlobalStatistics';
import AdminStationDetails from '../components/admin/AdminStationDetails';
import FuelTypeManager from '../components/admin/FuelTypeManager';
import logoImg from '../assets/logo.png';
import textLogoImg from '../assets/text_logo.jpg';
import './AdminDashboard.css'; 

const AdminDashboard = () => {
    const { logout } = useAuth();
    const [activeTab, setActiveTab] = useState('stations');
    const [inspectStationId, setInspectStationId] = useState(null);

    const handleInspect = (id) => {
        setInspectStationId(id);
    };

    const handleBackFromInspect = () => {
        setInspectStationId(null);
    };

    const renderContent = () => {
        if (activeTab === 'stations') {
            if (inspectStationId) {
                return <AdminStationDetails stationId={inspectStationId} onBack={handleBackFromInspect} />;
            }
            return <GasStationManager onInspect={handleInspect} />;
        }
        
        switch (activeTab) {
            case 'fueltypes': return <FuelTypeManager />;
            case 'users': return <UserManager />;
            case 'statistics': return <GlobalStatistics />;
            default: return <GasStationManager onInspect={handleInspect} />;
        }
    };

    return (
        <div className="admin-dashboard-container bg-light">
            <aside className="admin-sidebar bg-white shadow-sm border-end border-danger border-2">
                <div className="sidebar-header d-flex flex-column align-items-center py-4 border-bottom border-warning">
                    <img src={logoImg} alt="Shell Logo" height="60" className="mb-2" />
                    <img src={textLogoImg} alt="Shell" height="30" />
                    <p className="mt-2 text-danger fw-bold mb-0">Admin Dashboard</p>
                </div>
                <nav className="sidebar-nav">
                    <button 
                        className={`nav-btn ${activeTab === 'stations' ? 'active' : ''}`}
                        onClick={() => { setActiveTab('stations'); setInspectStationId(null); }}
                    >
                        Gas Stations
                    </button>
                    <button 
                        className={`nav-btn ${activeTab === 'fueltypes' ? 'active' : ''}`}
                        onClick={() => { setActiveTab('fueltypes'); setInspectStationId(null); }}
                    >
                        Fuel Types
                    </button>
                    <button 
                        className={`nav-btn ${activeTab === 'users' ? 'active' : ''}`}
                        onClick={() => { setActiveTab('users'); setInspectStationId(null); }}
                    >
                        Users & Roles
                    </button>
                    <button 
                        className={`nav-btn ${activeTab === 'statistics' ? 'active' : ''}`}
                        onClick={() => { setActiveTab('statistics'); setInspectStationId(null); }}
                    >
                        Global Statistics
                    </button>
                </nav>
                <div className="sidebar-footer">
                    <button className="btn logout-btn" onClick={logout}>Logout</button>
                </div>
            </aside>
            <main className="admin-main-content">
                <div className="card shadow-sm border-0 content-card">
                    <div className="card-body">
                        {renderContent()}
                    </div>
                </div>
            </main>
        </div>
    );
};

export default AdminDashboard;
