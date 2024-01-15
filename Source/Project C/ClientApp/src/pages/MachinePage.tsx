import { useEffect, useState } from 'react';
import { useClerk } from "@clerk/clerk-react";
import { machineService } from '../services/machineService';
import { Machine } from '../models/Machine';
import { CreateMachine } from '../models/CreateMachine';

const tokenType = 'api_token';

export default function MachinePage() {
    const [result, setResult] = useState<Machine[]>();
    const [single, setSingle] = useState<Machine>();
    const clerk = useClerk();

    // toJSON and validate methods


    useEffect(() => {
        async function fetchDataAsync() {
            try {
                const token = await clerk.session?.getToken({ template: tokenType });
                const service = new machineService();
                console.log(token)
                if (token) {
                    // Get all machines
                    const data = await service.getAll(token);
                    setResult(data);
                    data?.forEach((item) => console.log(item.toJSON()));

                    // public id?: number;
                    // public createdAt?: Date;
                    // public createdBy?: string;
                    // public updatedAt?: Date;
                    // public updatedBy?: string;
                    // public name?: string;
                    // public description?: string;

                    // Create a new machine
                    const newMachineData = {
                        createdBy: 'Test User',
                        description: 'Test Description',
                        name: 'Test Name',
                    };
                    const newMachine = CreateMachine.fromJson(newMachineData);
                    const createdMachine = await service.create(token, newMachine);

                    // Update an existing machine
                    if (createdMachine && createdMachine.id) {
                        createdMachine.description = 'Updated Description';
                        const updatedMachine = await service.update(token, createdMachine);
                        setSingle(updatedMachine);
                        console.log(updatedMachine);
                    }

                    // Delete a machine
                    if (createdMachine && createdMachine.id) {
                        await service.delete(token, createdMachine.id);
                        const deletedMachine = await service.getById(token, createdMachine.id);
                        setSingle(deletedMachine);
                        console.log(deletedMachine);
                    }
                } else {
                    console.error('Token not retrieved');
                }
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        }

        fetchDataAsync();
    }, [clerk.session]);

    return (
        <div>
            {/* Render your UI components here */}
        </div>
    );
}
