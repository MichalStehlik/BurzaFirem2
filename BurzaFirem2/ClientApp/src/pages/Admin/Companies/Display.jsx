import DateTime from "../../../components/DateTime"

const Display = ({data}) => {
    return (
        <>
        <h2>Popis firmy</h2>
        {data.description ? <div dangerouslySetInnerHTML={{__html: data.description }} /> : <p>nic</p>}      
        <h2>Nabídka</h2>
        {data.offer ? <div dangerouslySetInnerHTML={{__html: data.offer }} /> : <p>nic</p>}    
        <h2>Požadavek</h2>
        {data.wanted ? <div dangerouslySetInnerHTML={{__html: data.wanted }} /> : <p>nic</p>}    
        <h2>Adresa</h2>
        <p>{data.addressStreet}</p>
        <p>{data.municipality}</p>
        <h2>Prezentace</h2>
        <p>{"WEB: " + data.companyUrl}</p>
        <p>{"Jiné: " + data.presentationUrl}</p>
        <h2>Datumy</h2>
        <p>Vytvoření: <DateTime date={data.created}/></p>
        <p>Aktualizace: <DateTime date={data.updated}/></p>
        </>
    )
}

export default Display;