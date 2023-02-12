import { Outlet } from "react-router-dom"
//import {useRequireAdmin} from "../../../hooks/useRequireAdmin"
import {requireAdmin} from "../../../hoc/requireAdmin"

export const UsersLayout = () => {
    //useRequireAdmin();
    return ( <Outlet /> );
}

export default requireAdmin(UsersLayout);