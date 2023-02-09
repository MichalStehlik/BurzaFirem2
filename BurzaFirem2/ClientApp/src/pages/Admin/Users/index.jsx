import { Outlet } from "react-router-dom"
import {useRequireAdmin} from "../../../hooks/useRequireAdmin"

export const UsersLayout = () => {
    useRequireAdmin();
    return ( <Outlet /> );
}

export default UsersLayout;