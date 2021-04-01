import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-detalle-giro',
  templateUrl: './detalle-giro.component.html',
  styleUrls: ['./detalle-giro.component.scss']
})
export class DetalleGiroComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    listaEstrategiaPago: Dominio[] = [];
    dataHistorial: any[] = [];
    dataSource = new MatTableDataSource();
    dataSourceFuentes = new MatTableDataSource();
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
    displayedColumns: string[]  = [
        'drp',
        'numeroDrp',
        'nombreAportante',
        'porcentaje'
    ];
    displayedColumnsFuentes: string[]  = [
        'nombre',
        'fuenteRecursos',
        'saldoActualRecursos',
        'saldoValorFacturado'
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
    dataTable = [
        {
            numeroDrp: 'IP_00090',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    porcentaje: 70

                },
                {
                    nombre: 'Fundación Pies Descalzos',
                    porcentaje: 30

                }
            ]
        },
        {
            numeroDrp: 'IP_00123',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    porcentaje: 100

                }
            ]
        }
    ];
    dataTableFuentes = [
        {
            nombre: 'Alcaldía de Susacón',
            fuenteRecursos: 'Contingencias',
            saldoActualRecursos: 75000000,
            saldoValorFacturado: 75000000
        }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaEstrategiasPago()
            .subscribe( response => this.listaEstrategiaPago = response );
    }

    ngOnInit(): void {
        this.dataHistorial = [
            {
                fechaCreacion: new Date(),
                responsable: 'Coordinador financiera',
                observacion: '<p>test historial</p>'
            }
        ];
        this.dataSource = new MatTableDataSource( this.dataTable );
        this.dataSourceFuentes = new MatTableDataSource( this.dataTableFuentes );
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
    }

    getEstrategiaPago( codigo: string ) {
        if ( this.listaEstrategiaPago.length > 0 ) {
            const estrategia = this.listaEstrategiaPago.find( estrategia => estrategia.codigo === codigo );

            if ( estrategia !== undefined ) {
                return estrategia.nombre;
            }
        }
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
