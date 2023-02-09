import DateTime from "../../../components/DateTime"

const Display = ({data}) => {
    return (
        <>
        <h2>Informace</h2>
        <p>Email: {data.email}</p>
        <h2>Datumy</h2>
        <p>Vytvoření: <DateTime date={data.created}/></p>
        <p>Aktualizace: <DateTime date={data.updated}/></p>
        </>
    )
}

export default Display;