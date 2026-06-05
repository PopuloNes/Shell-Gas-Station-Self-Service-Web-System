import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { useNavigate } from 'react-router-dom';
import L from 'leaflet';
import api from '../services/api';
import { useAuth } from '../contexts/AuthContext';
import SettingsModal from '../components/SettingsModal';

import logoImg from '../assets/logo.png';
import textLogoImg from '../assets/text_logo.jpg';
import settingsIcon from '../assets/settings.png';

// Fix Leaflet's default icon path issue
import iconUrl from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';
let DefaultIcon = L.icon({
    iconUrl,
    shadowUrl: iconShadow,
    iconAnchor: [12, 41]
});
L.Marker.prototype.options.icon = DefaultIcon;

const MapSelection = () => {
    const [stations, setStations] = useState([]);
    const [showSettings, setShowSettings] = useState(false);
    const navigate = useNavigate();
    const { user } = useAuth();

    useEffect(() => {
        const fetchStations = async () => {
            try {
                const res = await api.get('/GasStation');
                setStations(res.data);
            } catch (err) {
                console.error("Failed to load gas stations", err);
            }
        };
        fetchStations();
    }, []);

    return (
        <div className="d-flex flex-column vh-100 bg-light">
            <header className="bg-white shadow-sm p-3 d-flex justify-content-between align-items-center" style={{ borderBottom: '4px solid var(--primary-color)' }}>
                <div className="d-flex align-items-center">
                    <img src={logoImg} alt="Shell Logo" height="50" className="me-2" />
                    <img src={textLogoImg} alt="Shell" height="30" />
                </div>
                
                <div className="d-flex align-items-center gap-3">
                    <div className="text-end">
                        <div className="fw-bold text-dark">{user?.username}</div>
                        <div className="text-warning fw-bold bg-dark px-2 rounded-pill small">
                            {user?.bonusBalance || 0} pts
                        </div>
                    </div>
                    
                    <button 
                        className="btn btn-link p-0" 
                        onClick={() => setShowSettings(true)}
                        style={{ border: 'none', background: 'transparent' }}
                    >
                        <img src={settingsIcon} alt="Settings" height="40" style={{ cursor: 'pointer', transition: 'transform 0.2s' }} onMouseOver={e => e.currentTarget.style.transform='rotate(45deg)'} onMouseOut={e => e.currentTarget.style.transform='rotate(0deg)'} />
                    </button>
                </div>
            </header>

            <SettingsModal show={showSettings} onHide={() => setShowSettings(false)} />
            
            <main className="flex-grow-1 p-3 d-flex flex-column">
                <h4 className="text-danger mb-3 fw-bold">Select Gas Station</h4>
                <div className="flex-grow-1 border border-danger border-2 rounded-4 overflow-hidden shadow">
                    <MapContainer center={[52.2297, 21.0122]} zoom={11} style={{ height: '100%', width: '100%' }}>
                        <TileLayer
                            url="https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png"
                            attribution='&copy; <a href="https://carto.com/">CARTO</a>'
                        />
                        
                        {stations.map(station => (
                            <Marker key={station.id} position={[station.latitude, station.longitude]}>
                                <Popup className="custom-popup">
                                    <div className="text-center" style={{ minWidth: '200px' }}>
                                        <h5 className="text-danger fw-bold mb-1">{station.name}</h5>
                                        <p className="text-muted small mb-3">{station.address}</p>
                                        
                                        {station.availableFuels && station.availableFuels.length > 0 && (
                                            <div className="bg-light p-2 rounded mb-3 text-start border border-warning">
                                                <strong className="d-block mb-2 small text-dark">Available Fuels:</strong>
                                                <ul className="list-unstyled m-0 small">
                                                    {station.availableFuels.map(f => {
                                                        const isAvailable = f.pumpIds && station.pumps?.some(p => p.status !== 2 && f.pumpIds.includes(p.id));
                                                        return (
                                                            <li key={f.fuelTypeId} className={`d-flex justify-content-between mb-1 ${!isAvailable ? 'text-muted text-decoration-line-through' : 'text-dark fw-bold'}`}>
                                                                <span>{f.fuelName}</span>
                                                                {isAvailable ? (
                                                                    <span className="text-danger">{f.price.toFixed(2)} PLN/L</span>
                                                                ) : (
                                                                    <span className="text-danger opacity-75">N/A</span>
                                                                )}
                                                            </li>
                                                        );
                                                    })}
                                                </ul>
                                            </div>
                                        )}

                                        <button 
                                            className="btn btn-warning w-100 fw-bold text-dark" 
                                            onClick={() => navigate(`/client/${station.id}`)}
                                        >
                                            Refuel Here
                                        </button>
                                    </div>
                                </Popup>
                            </Marker>
                        ))}
                    </MapContainer>
                </div>
            </main>
        </div>
    );
};

export default MapSelection;
