import { useState } from 'react';
import axios from 'axios';
import { useClerk } from "@clerk/clerk-react";

const MachineInfoForm = () => {
    const [machineId, setMachineId] = useState('');
    const [machineInfo, setMachineInfo] = useState(null);
    const tokenType = "api_token";
    const clerk = useClerk();

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = await clerk.session?.getToken({ template: tokenType });

        axios.get(`http://localhost:5069/Machine/${machineId}`, { headers: { Authorization: `Bearer ${token}` } })
            .then((response) => {
                setMachineInfo(response.data);
                console.log(response);
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
                    <div className="card-header">Machine: {machineInfo.name}</div>
                    <div className="card-body">
                        <ul className="list-group">
                            <li className="list-group-item">Machine ID: {machineInfo.id}</li>
                            <li className="list-group-item">Machine Description: {machineInfo.description}</li>
                            <li className="list-group-item">Date of creation: {machineInfo.createdAt}</li>
                            <li className="list-group-item">Created by: {machineInfo.createdBy}</li>
                        </ul>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MachineInfoForm;