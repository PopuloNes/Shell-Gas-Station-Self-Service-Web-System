import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import Login from './pages/Login';
import MapSelection from './pages/MapSelection';
import ClientDashboard from './pages/ClientDashboard';
import AdminDashboard from './pages/AdminDashboard';
import ManagerDashboard from './pages/ManagerDashboard';

const ProtectedRoute = ({ children, roles }) => {
    const { user, loading } = useAuth();
    if (loading) return <div>Loading...</div>;
    if (!user) return <Navigate to="/login" />;
    
    // Check role if specified
    if (roles && !roles.includes(user.role)) {
        return <Navigate to="/" />; // Redirect to map/home if unauthorized
    }
    
    return children;
};

const RoleBasedRedirect = () => {
    const { user } = useAuth();
    if (!user) return <Navigate to="/login" />;
    if (user.role === 'Admin') return <Navigate to="/admin" />; // Admin
    if (user.role === 'Manager') return <Navigate to="/manager" />; // Manager
    return <Navigate to="/map" />; // Client defaults to map
};

function App() {
    return (
        <AuthProvider>
            <div className="app-container" data-theme="dark">
                <Router>
                    <Routes>
                        <Route path="/" element={<RoleBasedRedirect />} />
                        <Route path="/login" element={<Login />} />
                        
                        <Route path="/map" element={
                            <ProtectedRoute roles={['Client']}>
                                <MapSelection />
                            </ProtectedRoute>
                        } />
                        
                        <Route path="/client/:stationId" element={
                            <ProtectedRoute roles={['Client']}>
                                <ClientDashboard />
                            </ProtectedRoute>
                        } />

                        <Route path="/admin" element={
                            <ProtectedRoute roles={['Admin']}>
                                <AdminDashboard />
                            </ProtectedRoute>
                        } />

                        <Route path="/manager" element={
                            <ProtectedRoute roles={['Manager']}>
                                <ManagerDashboard />
                            </ProtectedRoute>
                        } />
                        
                        <Route path="*" element={<Navigate to="/" />} />
                    </Routes>
                </Router>
            </div>
        </AuthProvider>
    );
}

export default App;
