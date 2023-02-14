import { Outlet } from "react-router-dom"
import {requireAuth} from "../../../hoc/requireAuth"

export const ImagesLayout = () => {
    return ( <Outlet /> );
}

export default requireAuth(ImagesLayout);