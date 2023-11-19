import { useEffect, useState } from 'react';
import SideBar from '../components/SideBar';
import { TicketService } from '../services/ticketService';
import { useClerk } from '@clerk/clerk-react';
import { Ticket } from '../models/Ticket';
import { CreateTicket } from '../models/CreateTicket';
import { Priority } from '../models/Priority';
import { Status } from '../models/Status';

const tokenType = 'api_token';

function renderArray(result: Ticket[] | undefined) {
    if (result) {
        return (
            <div>
                Data:
                {result.map((item, index) => (
                    <div key={index} className="mb-4">
                        <h3>Item {index + 1}</h3>
                        <ul>
                            {Object.entries(item).map(([key, value]) => (
                                <li key={key}>
                                    <strong>{key}:</strong> {typeof value === 'object' && value instanceof Date ? value.toString() : value}
                                </li>
                            ))}
                        </ul>
                    </div>
                ))}
            </div>
        );
    } else {
        return (
            <div>
                Not Loaded
            </div>
        );
    }
}

function renderSingle(single: Ticket | undefined) {
    if (single) {
        return (
            <div>
                Data:
                {Object.entries(single).map(([key, value]) => (
                    <li key={key}>
                        <strong>{key}:</strong> {typeof value === 'object' && value instanceof Date ? value.toString() : value}
                    </li>
                ))}
            </div>
        );
    } else {
        return (
            <div>
                Not Loaded
            </div>
        );
    }
}

export default function ApiTest() {
    const [result, setResult] = useState<Ticket[]>();
    const [single, setSingle] = useState<Ticket>();
    const clerk = useClerk();

    useEffect(() => {
        async function fetchDataAsync() {
            try {
                const token = await clerk.session?.getToken({ template: tokenType });
                const service = new TicketService();

                if (token) {
                    const data = await service.getAll(token);
                    setResult(data);
                    data?.forEach((item) => console.log(item.toJSON()));
                } else {
                    console.error('Token not retrieved');
                }

                if (token) {
                    const ticket = new CreateTicket();
                    ticket.createdBy = 'Test User';
                    ticket.description = 'Test Description';
                    ticket.triedSolutions = ['Test Solutions'];
                    ticket.additionalNotes = 'Test Notes';
                    ticket.priority = Priority.Low;
                    ticket.status = Status.Open;

                    const model = await service.getById(token, 5);
                    if (model) {
                        model.description = 'Updated Description 3';
                        const data = await service.update(token, model);
                        setSingle(data);
                        console.log(data);
                    }

                }

                if (token) {
                    const ticket = new CreateTicket();
                    ticket.createdBy = 'Test User';
                    ticket.description = 'Test Description';
                    ticket.triedSolutions = ['Test Solutions'];
                    ticket.additionalNotes = 'Test Notes';
                    ticket.priority = Priority.Low;
                    ticket.status = Status.Open;

                    const data = await service.create(token, ticket);
                    if (data) {
                        if (data.id) {
                            await service.delete(token, data.id);
                            const result = await service.getById(token, data.id);
                            setSingle(result);
                            console.log(result);
                        }

                    }

                }
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchDataAsync();
    }, [clerk.session]);

    return (
        <>
            <div className="flex gap-10 bg-[#F8F8F8] min-h-screen">
                <SideBar />
                <div className="w-full bg-[#F8F8F8] mt-10">
                    <div className="flex flex-col gap-16 pr-5">
                        <main className="grid grid-cols-[repeat(4,minmax(100px,500px))] h-min gap-6">
                            <div>
                                <div className="text-black">
                                    {renderSingle(single)}
                                </div>
                            </div>
                        </main>
                    </div>
                </div>
            </div>
        </>
    );
};
