import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-criterios-pago',
  templateUrl: './form-criterios-pago.component.html',
  styleUrls: ['./form-criterios-pago.component.scss']
})
export class FormCriteriosPagoComponent implements OnInit {

    addressForm = this.fb.group({
      criterioPago: [null, Validators.required],
      tipoPago: [null, Validators.required],
      conceptoPago: [null, Validators.required],
      valorFacturado: [null, Validators.required],
      criterios: this.fb.array( [] )
    });
    criteriosArray = [
      { name: 'Estudios y diseños interventoria hasta 90%', value: '1' },
      { name: 'Criterio 2', value: '2' },
      { name: 'Criterio 3', value: '3' },
      { name: 'Criterio 4', value: '4' },
      { name: 'Criterio 5', value: '5' },
    ];
    criteriosCodigo = {
      estudio: '1',
      criterio2: '2',
      criterio3: '3',
      criterio4: '4',
      criterio5: '5'
    }
    tipoPagoArray = [
      { name: 'Costo variable', value: '1' },
      { name: 'Tipo de pago 2', value: '2' },
      { name: 'Tipo de pago 3', value: '3' },
      { name: 'Tipo de pago 4', value: '4' },
      { name: 'Tipo de pago 5', value: '5' },
    ];
    conceptoPagoArray = [
      { name: '718100002002- DEMOLICIONES', value: '1' }
    ];
    obj1: boolean;

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);

        return alphanumeric.test(inputChar) ? true : false;
    }

    getvalues(values: string[]) {
        const tieneEstudios = values.includes( this.criteriosCodigo.estudio );
        const tieneCriterio2 = values.includes( this.criteriosCodigo.criterio2 );
        const tieneCriterio3 = values.includes( this.criteriosCodigo.criterio3 );
        const tieneCriterio4 = values.includes( this.criteriosCodigo.criterio4 );
        const tieneCriterio5 = values.includes( this.criteriosCodigo.criterio5 );
        const criteriosArray: any[] = this.criterios.value;

        // Criterio estudios
        if ( tieneEstudios === true && criteriosArray.filter( criterio => criterio.codigo === this.criteriosCodigo.estudio ).length === 0 ) {
            this.criterios.push(
                this.fb.group(
                    {
                        codigo: [ '1' ],
                        estadoCodigo: [ 0 ],
                        nombreCriterio: [ 'Estudios y diseños interventoria hasta 90%' ],
                        tipoPago: [ null, Validators.required ],
                        conceptoPago: [ null, Validators.required ],
                        valorFacturado: [ null, Validators.required ]
                    }
                )
            );
        }
        if ( tieneEstudios === false ) {
            this.criterios.controls.forEach( ( criterio, index ) => {
                if ( criterio.value.codigo === this.criteriosCodigo.estudio ) {
                    this.criterios.removeAt( index );
                }
            } );
        }
        // Criterio 2
        if ( tieneCriterio2 === true && criteriosArray.filter( criterio => criterio.codigo === this.criteriosCodigo.criterio2 ).length === 0 ) {
            this.criterios.push(
                this.fb.group(
                    {
                        codigo: [ '2' ],
                        estadoCodigo: [ 0 ],
                        nombreCriterio: [ 'Criterio 2' ],
                        tipoPago: [ null, Validators.required ],
                        conceptoPago: [ null, Validators.required ],
                        valorFacturado: [ null, Validators.required ]
                    }
                )
            );
        }
        if ( tieneCriterio2 === false ) {
            this.criterios.controls.forEach( ( criterio, index ) => {
                if ( criterio.value.codigo === this.criteriosCodigo.criterio2 ) {
                    this.criterios.removeAt( index );
                }
            } );
        }
        // Criterio 3
        if ( tieneCriterio3 === true && criteriosArray.filter( criterio => criterio.codigo === this.criteriosCodigo.criterio3 ).length === 0 ) {
            this.criterios.push(
                this.fb.group(
                    {
                        codigo: [ '3' ],
                        estadoCodigo: [ 0 ],
                        nombreCriterio: [ 'Criterio 3' ],
                        tipoPago: [ null, Validators.required ],
                        conceptoPago: [ null, Validators.required ],
                        valorFacturado: [ null, Validators.required ]
                    }
                )
            );
        }
        if ( tieneCriterio3 === false ) {
            this.criterios.controls.forEach( ( criterio, index ) => {
                if ( criterio.value.codigo === this.criteriosCodigo.criterio3 ) {
                    this.criterios.removeAt( index );
                }
            } );
        }
        // Criterio 4
        if ( tieneCriterio4 === true && criteriosArray.filter( criterio => criterio.codigo === this.criteriosCodigo.criterio4 ).length === 0 ) {
            this.criterios.push(
                this.fb.group(
                    {
                        codigo: [ '4' ],
                        estadoCodigo: [ 0 ],
                        nombreCriterio: [ 'Criterio 4' ],
                        tipoPago: [ null, Validators.required ],
                        conceptoPago: [ null, Validators.required ],
                        valorFacturado: [ null, Validators.required ]
                    }
                )
            );
        }
        if ( tieneCriterio4 === false ) {
            this.criterios.controls.forEach( ( criterio, index ) => {
                if ( criterio.value.codigo === this.criteriosCodigo.criterio4 ) {
                    this.criterios.removeAt( index );
                }
            } );
        }
        // Criterio 5
        if ( tieneCriterio5 === true && criteriosArray.filter( criterio => criterio.codigo === this.criteriosCodigo.criterio5 ).length === 0 ) {
            this.criterios.push(
                this.fb.group(
                    {
                        codigo: [ '5' ],
                        estadoCodigo: [ 0 ],
                        nombreCriterio: [ 'Criterio 5' ],
                        tipoPago: [ null, Validators.required ],
                        conceptoPago: [ null, Validators.required ],
                        valorFacturado: [ null, Validators.required ]
                    }
                )
            );
        }
        if ( tieneCriterio5 === false ) {
            this.criterios.controls.forEach( ( criterio, index ) => {
                if ( criterio.value.codigo === this.criteriosCodigo.criterio5 ) {
                    this.criterios.removeAt( index );
                }
            } );
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    deleteCriterio( index: number, estadoCodigo: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        if ( estadoCodigo === 0 ) {
                            this.criterios.removeAt( index );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            this.criterios.removeAt( index );
                            this.openDialog( '', '<b>Falta el servicio.</b>' );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
                    }
                }
            );
    }

    onSubmit() {
    }

}
