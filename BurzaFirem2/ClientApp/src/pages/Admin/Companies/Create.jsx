import {useState} from "react"
import { Button, Form, FormGroup, Label, Input, Alert, Spinner } from 'reactstrap';
import { useForm, Controller } from "react-hook-form"
import { useAuthContext } from "../../../providers/AuthProvider"
import { useNavigate } from "react-router-dom"
import {CKEditor} from '@ckeditor/ckeditor5-react'
import Editor from '@ckeditor/ckeditor5-build-classic'
import axios from "axios";

const Create = props => {
    const { register, handleSubmit, control, setValue, getValues, formState: { errors } } = useForm({
        defaultValues: {
          name: "",
          addressStreet: "",
          municipality: "",
          companyUrl: "",
          presentationUrl: "",
          description: "",
          offer: "",
          wanted: "",
          companyBranches: ""
    }})
    const [{ accessToken }] = useAuthContext();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [failed, setFailed] = useState(false);
    const [ok, setOk] = useState(false);
    const onSubmit = data => {
        setIsLoading(true);
        setOk(false);
        setFailed(false);
        axios.post("/api/v1/companies", {
            name: data.name.trim(),
            addressStreet: data.addressStreet,
            municipality: data.municipality,
            companyUrl: data.companyUrl,
            presentationUrl: data.presentationUrl,
            description: data.description,
            offer: data.offer,
            companyBranches: data.companyBranches,
            wanted: data.wanted
        }, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            setOk(true);
            setFailed(false);
            navigate("/admin/companies/" + response.data.companyId)
        })
        .catch(error => {
            setOk(false);
            setFailed(true);
        })
        .then(() => {
            setIsLoading(false);
        })
    };
    let {description, offer, wanted, companyBranches} = getValues();
    return (
        <>
            <h1>Vytvoření nové firmy</h1>
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="name">Název</Label>
                    <Controller name="name" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} placeholder="The Company" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Název je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="name">Ulice a č.p.</Label>
                    <Controller name="addressStreet" control={control} render={({ field }) => <Input {...field} placeholder="Masarykova 3" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="name">Obec a PSČ</Label>
                    <Controller name="municipality" rules={{ required: true }} control={control} render={({ field }) => <Input {...field} placeholder="Liberec 460 03" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="name">Webová stránka společnosti</Label>
                    <Controller name="companyUrl" control={control} render={({ field }) => <Input {...field} placeholder="https://company.cz" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="name">Odkaz na prezentaci společnosti</Label>
                    <Controller name="presentationUrl" control={control} render={({ field }) => <Input {...field} placeholder="https://company.cz/presentation" />} />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="name">Popis společnosti</Label>
                    <Controller name="description" control={control} render={({ field }) => 
                    <CKEditor
                            editor={ Editor }
                            data={description}
                            config={{toolbar: ['bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote' ], placeholder: "Naše společnost ..."}}
                            onReady={ editor => {
                            } }
                            onChange={ ( event, editor ) => {
                                const data = editor.getData();
                                setValue("description", data);
                            } }
                            onBlur={ ( event, editor ) => {
                            } }
                            onFocus={ ( event, editor ) => {
                            } }
                        />} />
                </FormGroup>
                <FormGroup>
                    <Label for="name">Nabídka</Label>
                    <Controller name="offer" control={control} render={({ field }) => 
                    <CKEditor
                            editor={ Editor }
                            data={offer}
                            config={{toolbar: ['bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote' ], placeholder: "Nabízíme ..."}}
                            onReady={ editor => {
                            } }
                            onChange={ ( event, editor ) => {
                                const data = editor.getData();
                                setValue("offer", data);
                            } }
                            onBlur={ ( event, editor ) => {
                            } }
                            onFocus={ ( event, editor ) => {
                            } }
                        />} />
                </FormGroup>
                <FormGroup>
                    <Label for="name">Požadavek</Label>
                    <Controller name="description" control={control} render={({ field }) => 
                    <CKEditor
                            editor={ Editor }
                            data={wanted}
                            config={{toolbar: ['bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote' ], placeholder: "Hledáme ..."}}
                            onReady={ editor => {
                            } }
                            onChange={ ( event, editor ) => {
                                const data = editor.getData();
                                setValue("wanted", data);
                            } }
                            onBlur={ ( event, editor ) => {
                            } }
                            onFocus={ ( event, editor ) => {
                            } }
                        />} />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="name">Pobočky</Label>
                    <Controller name="companyBranches" control={control} render={({ field }) => 
                    <CKEditor
                            editor={ Editor }
                            data={companyBranches}
                            config={{toolbar: ['bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote' ], placeholder: ""}}
                            onReady={ editor => {
                            } }
                            onChange={ ( event, editor ) => {
                                const data = editor.getData();
                                setValue("companyBranches", data);
                            } }
                            onBlur={ ( event, editor ) => {
                            } }
                            onFocus={ ( event, editor ) => {
                            } }
                        />} />
                </FormGroup>
                {
                failed
                ?
                <Alert color="danger">Při ukládání dat došlo k chybě.</Alert>
                :
                null
                }
                <div className="my-2">
                {
                    isLoading
                    ?
                    <Spinner className="m-2"> </Spinner>
                    :
                    <>
                        <Button type="submit" color="primary" className="m-1">Uložit</Button>
                        <Button onClick={e => { navigate("/admin/companies")}}>Zpět</Button>
                    </>
                }            
                </div>
            </form>
        </>
    );
}

export default Create;