import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-avance-financiero',
  templateUrl: './avance-financiero.component.html',
  styleUrls: ['./avance-financiero.component.scss']
})
export class AvanceFinancieroComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() avanceFinancieroObs: string;
    formAvanceFinanciero: FormGroup = this.fb.group({
      tieneObservaciones: [ null, Validators.required ],
      observaciones: [ null ],
      fechaCreacion: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFinancieroId: number;
    seguimientoSemanalObservacionId = 0;
    avanceFinanciero: any;
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

    constructor(
        private fb: FormBuilder,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private dialog: MatDialog,
        private routes: Router,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService )
    { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalAvanceFinancieroId =  this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0].seguimientoSemanalAvanceFinancieroId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
                this.avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];
                if ( this.avanceFinanciero.observacionApoyoId !== undefined ) {
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalAvanceFinancieroId, this.avanceFinancieroObs )
                        .subscribe(
                            response => {
                                const obsFinanciero = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                if ( obsFinanciero[0].observacion !== undefined ) {
                                    if ( obsFinanciero[0].observacion.length > 0 ) {
                                        this.formAvanceFinanciero.get( 'observaciones' ).setValue( obsFinanciero[0].observacion );
                                    }
                                }
                                this.dataHistorial = response.filter( obs => obs.archivada === true );
                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                this.seguimientoSemanalObservacionId = obsFinanciero[0].seguimientoSemanalObservacionId;
                                this.formAvanceFinanciero.get( 'tieneObservaciones' ).setValue( this.avanceFinanciero.tieneObservacionApoyo );
                                this.formAvanceFinanciero.get( 'fechaCreacion' ).setValue( obsFinanciero[0].fechaCreacion );
                            }
                        )
                }
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formAvanceFinanciero.get( 'tieneObservaciones' ).value === false && this.formAvanceFinanciero.get( 'observaciones' ).value !== null ) {
            this.formAvanceFinanciero.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.avanceFinancieroObs,
            observacionPadreId: this.seguimientoSemanalAvanceFinancieroId,
            observacion: this.formAvanceFinanciero.get( 'observaciones' ).value,
            tieneObservacion: this.formAvanceFinanciero.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'False' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
