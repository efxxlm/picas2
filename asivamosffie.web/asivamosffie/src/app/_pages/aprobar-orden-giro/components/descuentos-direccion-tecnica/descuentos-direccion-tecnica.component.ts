import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-descuentos-direccion-tecnica',
  templateUrl: './descuentos-direccion-tecnica.component.html',
  styleUrls: ['./descuentos-direccion-tecnica.component.scss']
})
export class DescuentosDireccionTecnicaComponent implements OnInit {

    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    dataSource = new MatTableDataSource();
    dataHistorial: any[] = [];
    tablaHistorial = new MatTableDataSource();
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    displayedColumns: string[]  = [
        'tipoDescuento',
        'valorDescuento',
        'valorTotalDescuento',
        'valorNetoGiro'
    ];
    dataTable = [
        {
            tipoDescuento: '4 x mil',
            valorDescuento: 60000,
            valorTotalDescuento: 60000,
            valorNetoGiro: 14940000
        }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.dataHistorial = [
            {
                fechaCreacion: new Date(),
                responsable: 'Coordinador financiera',
                observacion: '<p>test historial</p>'
            }
        ];
        this.dataSource = new MatTableDataSource( this.dataTable );
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formObservacion.get( 'tieneObservaciones' ).value === false && this.formObservacion.get( 'observaciones' ).value !== null ) {
            this.formObservacion.get( 'observaciones' ).setValue( '' );
        }
        console.log( this.formObservacion );
    }

}
