import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-social',
  templateUrl: './gestion-social.component.html',
  styleUrls: ['./gestion-social.component.scss']
})
export class GestionSocialComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionSocial: any;
    formGestionSocial: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionSocial: any;
    seguimientoSemanalGestionObraSocialId = 0;
    seguimientoSemanalObservacionId = 0;
    observacionApoyo: any[] = [];
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
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial.length > 0 )
            {
                this.gestionSocial = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
                if ( this.gestionSocial !== undefined ) {
                    this.seguimientoSemanalGestionObraSocialId = this.gestionSocial.seguimientoSemanalGestionObraSocialId;
                    if ( this.gestionSocial.observacionApoyoId !== undefined || this.gestionSocial.observacionSupervisorId !== undefined ) {
                        this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalGestionObraSocialId, this.tipoObservacionSocial )
                            .subscribe(
                                response => {
                                    this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                    const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                    this.dataHistorial = response.filter( obs => obs.archivada === true );
                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                    if ( observacionSupervisor.length > 0 ) {
                                        if ( observacionSupervisor[0].observacion !== undefined ) {
                                            if ( observacionSupervisor[0].observacion.length > 0 ) {
                                                this.formGestionSocial.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                            }
                                        }
                                        this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                        this.formGestionSocial.get( 'tieneObservaciones' ).setValue( this.gestionSocial.tieneObservacionSupervisor );
                                        this.formGestionSocial.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
                                    }
                                }
                            );
                    }
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
        if ( this.formGestionSocial.get( 'tieneObservaciones' ).value === false && this.formGestionSocial.get( 'observaciones' ).value !== null ) {
            this.formGestionSocial.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionSocial,
            observacionPadreId: this.seguimientoSemanalGestionObraSocialId,
            observacion: this.formGestionSocial.get( 'observaciones' ).value,
            tieneObservacion: this.formGestionSocial.get( 'tieneObservaciones' ).value,
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
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
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
