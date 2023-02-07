import React, { useState, useEffect, useCallback } from 'react'
import { Spinner, Alert, ListGroup, ListGroupItem, Badge, Button, Input, Form } from 'reactstrap';
import axios from "axios"
import { Link } from 'react-router-dom'

export const Home = () =>  {
    const [data, setData] = useState(null)
    const [error, setError] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [selectedBranches, setSelectedBranches] = useState([]);
    const [selectedActivities, setSelectedActivities] = useState([]);
    const [selectedList, setSelectedList] = useState([]);
    const [name, setName] = useState("");

    const fetchCompanies = useCallback(
        (selectedBranches, selectedActivities, name) => {
        setIsLoading(true);
        let headers = {
          "Content-Type": "application/json"
        }  
        axios.get("/api/v1/companies", { headers: headers, params: {order: "name", branches: selectedBranches.join(","), activities: selectedActivities.join(","), name} })
        .then(response => { 
            setData(response.data.data);
            setError(null); 
        })
        .catch(error => {
          setData(null);
          if (error.response) {
              setError({status: error.response.status, text: error.response.statusText})
          } else {
              setError({status: 0, text: "??"});
          }
        })
        .then(()=>{
            setIsLoading(false)
        })
    },[]);

    useEffect(() => {
      fetchCompanies(selectedBranches, selectedActivities, name);
    }, [selectedBranches, selectedActivities, name, fetchCompanies])

    return (
      <>
        <FilterForm 
          selectedBranches={selectedBranches} 
          setSelectedBranches={setSelectedBranches} 
          selectedActivities={selectedActivities} 
          setSelectedActivities={setSelectedActivities} 
          name={name} 
          setName={setName} 
        />
        {isLoading
        ?
            <div className="text-center p-3"><Spinner className="m-2"> </Spinner></div>
        :
            error 
            ?
            <Alert color="danger">Při získávání dat došlo k chybě.</Alert>
            :
                data
                ?
                    data.length > 0
                    ?
                    <ListGroup>
                    {data.map((item, index) => (
                        <ListGroupItem key={index} tag={Link} to={"/" + item.companyId}>{item.name}
                            {item.branches.map((item, index) => (<Badge key={ index } style={{backgroundColor: `${item.backgroundColor}`, color: `${item.textColor}`, marginLeft: 5}}>{item.name.substr(0,3)}</Badge>))}
                        </ListGroupItem>
                    ))}
                    </ListGroup>
                    :
                    <Alert color="info" className="mt-2">Filtru neodpovídají žádné firmy.</Alert>
                :
                null
        }
      </>
    );
}

const FilterForm = ({selectedBranches, setSelectedBranches, selectedActivities, setSelectedActivities, name, setName}) => {
  const [branches, setBranches] = useState(null);
  const [errorActivities, setErrorActivities] = useState(false)
  const [isLoadingActivities, setIsLoadingActivities] = useState(false)
  const [activities, setActivities] = useState(null);
  const [isLoadingBranches, setIsLoadingBranches] = useState(false);
  const [errorBranches, setErrorBranches] = useState(false);

  const toggleSelectedBranches = (id) => {
    let newSelectedBranches = [...selectedBranches];
    if (newSelectedBranches.includes(id))
    {
        var index = newSelectedBranches.indexOf(id);
        if (index !== -1) {
            newSelectedBranches.splice(index, 1);
        }
    }
    else
    {
        newSelectedBranches.push(id);
    }
    setSelectedBranches(newSelectedBranches);
  }

  const toggleSelectedActivities = (id) => {
    let newSelectedActivities = [...selectedActivities];
    if (newSelectedActivities.includes(id))
    {
        var index = newSelectedActivities.indexOf(id);
        if (index !== -1) {
            newSelectedActivities.splice(index, 1);
        }
    }
    else
    {
        newSelectedActivities.push(id);
    }
    setSelectedActivities(newSelectedActivities);
  }

    useEffect(() => {
        const fetchBranches = () => {
            setIsLoadingBranches(true);
            setErrorBranches(false);
            axios.get("/api/v1/branches?visible=true", {
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .then(response => {
                    setBranches(response.data);
                })
                .catch(error => {
                    if (error.response) {
                        setErrorBranches({ status: error.response.status, text: error.response.statusText });
                    }
                    else {
                        setErrorBranches({ status: 0, text: "Neznámá chyba" });
                    }
                    setBranches([]);
                });
            setIsLoadingBranches(false);
        };

        const fetchActivities = () => {
            setIsLoadingActivities(true);
            setErrorActivities(false);
            axios.get("/api/v1/activities?visible=true", {
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .then(response => {
                    setActivities(response.data);
                })
                .catch(error => {
                    if (error.response) {
                        setErrorActivities({ status: error.response.status, text: error.response.statusText });
                    }
                    else {
                        setErrorActivities({ status: 0, text: "Neznámá chyba" });
                    }
                    setActivities([]);
                })
                .then(() => {
                    setIsLoadingActivities(false);
                })
        };
    fetchBranches();
    fetchActivities();
  },[])


  if (branches && activities) {
    return (
      <Form inline>
          <Input onChange={e => {setName(e.target.value)}} value={name} placeholder="Název nebo jeho část" />
          {branches.map((item, index) => {
            if (item.visible)
            return (
                  <Button 
                    key={index} 
                    className="m-1" 
                    outline={!selectedBranches.includes(item.branchId)} 
                    onClick={e => { toggleSelectedBranches(item.branchId) }} 
                    size="sm"
                    style={
                        selectedBranches.includes(item.branchId)
                            ?
                            { color: item.textColor, backgroundColor: item.backgroundColor }
                            :
                            { color: item.backgroundColor, backgroundColor: item.textColor }
                        } 
                    >{item.name}</Button>
            );
          }
          )}
          {activities.map((item, index) => {
            if (item.visible)
            return (
                <Button 
                key={index} 
                className="m-1" 
                outline={!selectedActivities.includes(item.activityId)} 
                onClick={e => {toggleSelectedActivities(item.activityId)}} 
                size="sm">{item.name}</Button>
            );
          }
              
          )}
      </Form>
    );
  }
}

export default Home;