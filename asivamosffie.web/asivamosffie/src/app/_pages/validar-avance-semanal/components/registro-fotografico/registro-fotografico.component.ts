import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registro-fotografico',
  templateUrl: './registro-fotografico.component.html',
  styleUrls: ['./registro-fotografico.component.scss']
})
export class RegistroFotograficoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoRegistroFotografico: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistroFotograficoId: number;
    seguimientoSemanalObservacionId = 0;
    reporteFotografico: any;
    observacionApoyo: any[] = [];
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    formRegistroFotografico: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
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

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalRegistroFotograficoId =  this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0].seguimientoSemanalRegistroFotograficoId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ) {
                this.reporteFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                if ( this.reporteFotografico.observacionApoyoId !== undefined ) {
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.reporteFotografico.seguimientoSemanalRegistroFotograficoId, this.tipoRegistroFotografico )
                        .subscribe(
                            response => {
                                this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.dataHistorial = response.filter( obs => obs.archivada === true );
                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                if ( observacionSupervisor.length > 0 ) {
                                    if ( observacionSupervisor[0].observacion !== undefined ) {
                                        if ( observacionSupervisor[0].observacion.length > 0 ) {
                                            this.formRegistroFotografico.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                        }
                                    }
                                    this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                    this.formRegistroFotografico.get( 'tieneObservaciones' ).setValue( this.reporteFotografico.tieneObservacionSupervisor );
                                    this.formRegistroFotografico.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
                                }
                            }
                        );
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
        if ( this.formRegistroFotografico.get( 'tieneObservaciones' ).value === false && this.formRegistroFotografico.get( 'observaciones' ).value !== null ) {
            this.formRegistroFotografico.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoRegistroFotografico,
            observacionPadreId: this.seguimientoSemanalRegistroFotograficoId,
            observacion: this.formRegistroFotografico.get( 'observaciones' ).value,
            tieneObservacion: this.formRegistroFotografico.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
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
