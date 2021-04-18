import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';

@Component({
  selector: 'app-avance-financiero',
  templateUrl: './avance-financiero.component.html',
  styleUrls: ['./avance-financiero.component.scss']
})
export class AvanceFinancieroComponent implements OnInit, OnDestroy {

    @Input() esVerDetalle = false;
    @Input() esRegistroNuevo: boolean;
    @Input() seguimientoSemanal: any;
    @Input() avanceFinancieroObs: string;
    seRealizoPeticion = false;
    formAvanceFinanciero: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFinancieroId: number;
    avanceFinanciero: any;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: []
    };
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
    }
    ngOnDestroy(): void {
        if ( this.formAvanceFinanciero.dirty === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataAvanceFinanciero( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataAvanceFinanciero( undefined )
        }
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalAvanceFinancieroId =  this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0].seguimientoSemanalAvanceFinancieroId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
                this.avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];

                if ( this.esVerDetalle === false ) {
                    this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalAvanceFinancieroId, this.avanceFinancieroObs )
                    .subscribe(
                        response => {
                            this.dataHistorial = response.filter( obs => obs.archivada === true );
                            this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                        }
                    )
                }

                this.formAvanceFinanciero.setValue(
                    {
                        requiereObservacion:    this.avanceFinanciero.requiereObservacion !== undefined ?
                                                this.avanceFinanciero.requiereObservacion : null,
                        observacion:    this.avanceFinanciero.observacion !== undefined ?
                                        this.avanceFinanciero.observacion : null,
                        generarAlerta:  this.avanceFinanciero.generarAlerta !== undefined ?
                                        this.avanceFinanciero.generarAlerta : null
                    }
                );
            }
        }
    }

    crearFormulario() {
        this.formAvanceFinanciero = this.fb.group({
            requiereObservacion: [ null ],
            observacion: [ null ],
            generarAlerta: [ null ]
        });
    }

    textoLimpio(texto: string) {
        if ( texto ){
            const textolimpio = texto.replace(/<[^>]*>/g, '');
            return textolimpio.length;
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
          e.editor.deleteText(n, e.editor.getLength());
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalAvanceFinanciero = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalAvanceFinancieroId: this.seguimientoSemanalAvanceFinancieroId,
                requiereObservacion:    this.formAvanceFinanciero.get( 'requiereObservacion' ).value !== null ?
                                        this.formAvanceFinanciero.get( 'requiereObservacion' ).value : null,
                observacion:    this.formAvanceFinanciero.get( 'observacion' ).value !== null ?
                                this.formAvanceFinanciero.get( 'observacion' ).value : null,
                generarAlerta:  this.formAvanceFinanciero.get( 'generarAlerta' ).value !== null ?
                                this.formAvanceFinanciero.get( 'generarAlerta' ).value : null
            }
        ];
        pSeguimientoSemanal.seguimientoSemanalAvanceFinanciero = seguimientoSemanalAvanceFinanciero;
        console.log( pSeguimientoSemanal );
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.seRealizoPeticion = true;
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardadoParcial() {
        const seguimientoSemanalAvanceFinanciero = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalAvanceFinancieroId: this.seguimientoSemanalAvanceFinancieroId,
                requiereObservacion:    this.formAvanceFinanciero.get( 'requiereObservacion' ).value !== null ?
                                        this.formAvanceFinanciero.get( 'requiereObservacion' ).value : null,
                observacion:    this.formAvanceFinanciero.get( 'observacion' ).value !== null ?
                                this.formAvanceFinanciero.get( 'observacion' ).value : null,
                generarAlerta:  this.formAvanceFinanciero.get( 'generarAlerta' ).value !== null ?
                                this.formAvanceFinanciero.get( 'generarAlerta' ).value : null
            }
        ];
        return seguimientoSemanalAvanceFinanciero;
    }

}
