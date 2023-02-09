import React, {useState} from 'react';
import { Button, FormGroup, Label, Input, Alert, Spinner } from 'reactstrap';
import { useForm, Controller } from "react-hook-form"
import { useAuthContext } from "../../../providers/AuthProvider"
import {CKEditor} from '@ckeditor/ckeditor5-react'
import Editor from '@ckeditor/ckeditor5-build-classic'
import axios from "axios"

const Edit = ({data, switchMode}) => {
    const { register, handleSubmit, control, setValue, getValues, formState: { errors } } = useForm({
        defaultValues: {
          name: data.name,
          addressStreet: data.addressStreet,
          municipality: data.municipality,
          companyUrl: data.companyUrl,
          presentationUrl: data.presentationUrl,
          description: data.description,
          offer: data.offer,
          wanted: data.wanted,
          companyBranches: data.companyBranches
    }})
    const [{ accessToken }] = useAuthContext();
    const [isLoading, setIsLoading] = useState(false);
    const [failed, setFailed] = useState(false);
    const [ok, setOk] = useState(false);
    const onSubmit = values => {
        console.log(values);
        setIsLoading(true);
        setOk(false);
        setFailed(false);
        axios.put("/api/v1/companies/" + data.companyId, {
            companyId: data.companyId,
            name: values.name.trim(),
            addressStreet: values.addressStreet,
            municipality: values.municipality,
            companyUrl: values.companyUrl,
            presentationUrl: values.presentationUrl,
            description: values.description,
            offer: values.offer,
            wanted: values.wanted,
            companyBranches: values.companyBranches
        }, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            setOk(true);
            setFailed(false);
            switchMode(false);
        })
        .catch(error => {
            setOk(false);
            setFailed(true);
        })
        .then(() => {
            setIsLoading(false);
        })
    };
    let {description, offer, wanted, logoOrientation, companyBranches} = getValues();
    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="name">Název</Label>
                    <Controller name="name" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} placeholder="The Company" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Název je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="addressStreet">Ulice a č.p.</Label>
                    <Controller name="addressStreet" control={control} render={({ field }) => <Input {...field} placeholder="Masarykova 3" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="municipality">Obec a PSČ</Label>
                    <Controller name="municipality" rules={{ required: true }} control={control} render={({ field }) => <Input {...field} placeholder="Liberec 460 03" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="companyUrl">Webová stránka společnosti</Label>
                    <Controller name="companyUrl" control={control} render={({ field }) => <Input {...field} placeholder="company.cz" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Adresa je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="presentationUrl">Odkaz na prezentaci společnosti</Label>
                    <Controller name="presentationUrl" control={control} render={({ field }) => <Input {...field} placeholder="company.cz/presentation" />} />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="description">Popis společnosti</Label>
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
                    <Label for="offer">Nabídka</Label>
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
                    <Label for="wanted">Požadavek</Label>
                    <Controller name="wanted" control={control} render={({ field }) => 
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
                    <Label for="companyBranches">Pobočky</Label>
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
                        <Button onClick={e => {switchMode(false)}}>Storno</Button>
                    </>
                }            
                </div>
            </form>
        </>
    );
}

export default Edit;