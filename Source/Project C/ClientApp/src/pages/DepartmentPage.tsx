import { useEffect, useState } from "react";
import { useClerk } from "@clerk/clerk-react";
import { departmentService } from "../services/departmentService";
import { Department } from "../models/Department";
import { CreateDepartment } from "@/models/CreateDepartment";
import { Button } from "@/components/ui/button";

const departmentMock = new CreateDepartment();
departmentMock.createdBy = "John Doe";
departmentMock.name = "My Department";
departmentMock.description = "This is a department description";

const departmentMock2 = new Department();
departmentMock2.id = 2;
departmentMock2.createdAt = new Date();
departmentMock2.createdBy = "Jiajun Doe";
departmentMock2.updatedAt = new Date();
departmentMock2.updatedBy = "Jiajun Doe";
departmentMock2.name = "Updated department";
departmentMock2.description = "Mock data for update";

const DepartmentPage = () => {
    const [departments, setDepartments] = useState<Department[]>();
    const clerk = useClerk();

    // ojecbt maken

    const renderTableHeader = () => {
        if (!departments || departments.length === 0) return null;

        // Get the property names
        const header = Object.keys(departments[0]);

        // Create table header cells
        return header.map((key) => <th key={key}>{key}</th>);
    };

    const renderTableData = () => {
        if (!departments || departments.length === 0) return null;

        return departments.map((department, index) => {
            return (
                <tr key={index}>
                    {Object.values(department).map((value, subIndex) => (
                        <td
                            key={subIndex}
                            style={{ padding: "8px", textAlign: "left" }}
                        >
                            {value instanceof Date
                                ? value.toLocaleString()
                                : value}
                        </td>
                    ))}
                </tr>
            );
        });
    };

    const addDepartment = async (department: CreateDepartment) => {
        const token = await clerk.session?.getToken({
            template: "api_token",
        });
        const service = new departmentService();

        if (token) {
            await service.create(token, department);
        } else {
            console.error("Token not retrieved");
        }
    };

    const updateDepartment = async (department: Department) => {
        const token = await clerk.session?.getToken({
            template: "api_token",
        });
        const service = new departmentService();

        if (token) {
            await service.update(token, department);
        } else {
            console.error("Token not retrieved");
        }
    };

    const deleteDepartment = async (departmentId: number) => {
        const token = await clerk.session?.getToken({
            template: "api_token",
        });
        const service = new departmentService();

        if (token) {
            await service.delete(token, departmentId);
        } else {
            console.error("Token not retrieved");
        }
    };

    useEffect(() => {
        async function fetchDataAsync() {
            const token = await clerk.session?.getToken({
                template: "api_token",
            });
            const service = new departmentService();

            console.log(token);

            if (token) {
                const data = await service.getAll(token);
                setDepartments(data);
                data?.forEach((item) => console.log(item.toJSON()));
            } else {
                console.error("Token not retrieved");
            }
        }

        fetchDataAsync();
    }, [clerk.session]);

    return (
        <div>
            <div className="flex space-x-10 m-8">
                <Button onClick={() => addDepartment(departmentMock)}>
                    Add department
                </Button>
                <Button onClick={() => updateDepartment(departmentMock2)}>
                    Update department
                </Button>
                <Button onClick={() => deleteDepartment(2)}>
                    Delete department
                </Button>
            </div>

            {departments ? (
                departments.length > 0 ? (
                    <table>
                        <thead>
                            <tr>{renderTableHeader()}</tr>
                        </thead>
                        <tbody>{renderTableData()}</tbody>
                    </table>
                ) : (
                    <p>No departments found.</p>
                )
            ) : (
                <p>Loading...</p>
            )}
        </div>
    );
};

export default DepartmentPage;
