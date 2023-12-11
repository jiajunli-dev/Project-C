import React, { useState } from 'react';
import axios from 'axios';

const MachineInfoForm = () => {
    const [machineId, setMachineId] = useState('');
    const [machineInfo, setMachineInfo] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();

        axios.get("Machine/${machineId}")
            .then((response) => {
                setMachineInfo(response.data);
            })
            .catch((error) => {
                console.error('Error fetching machine', error);
                setMachineInfo(null);
            });
    };

    return (
        <div className="container">
            <h1>Machine Information</h1>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="machineId">Machine ID</label>
                    <input
                        type="number"
                        id="machineId"
                        className="form-control"
                        placeholder="Enter machine ID"
                        value={machineId}
                        onChange={(e) => setMachineId(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary">
                    Search
                </button>
            </form>
            {machineInfo && (
                <div className="card mt-3">
                    <div className="card-header">{machineInfo.Name}</div>
                    <div className="card-body">
                        <ul className="list-group">
                            <li className="list-group-item">Machine ID: {machineInfo.Description}</li>
                        </ul>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MachineInfoForm;