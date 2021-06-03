import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import humanize from 'humanize-plus';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-revision-aprobacion-rapg',
  templateUrl: './revision-aprobacion-rapg.component.html',
  styleUrls: ['./revision-aprobacion-rapg.component.scss']
})
export class RevisionAprobacionRapgComponent implements OnInit {

    @Input() contratoPoliza: any;
    @Input() esVerDetalle: boolean;
    listaPerfilCodigo = PerfilCodigo;
    estadosRevision = EstadosRevision;
    contratoPolizaActualizacion: any;
    contadorRevision = 0;
    minDate = new Date();
    addressForm = this.fb.group({
        contratoPolizaActualizacionId: [ 0 ],
        contratoPolizaActualizacionRevisionAprobacionObservacionId: [ 0 ],
        fechaRevision: [ null, Validators.required ],
        estadoRevision: [ null, Validators.required ],
        fechaAprob: [ null, Validators.required],
        responsableAprob: [ null, Validators.required ],
        observacionesGenerales: [ null, Validators.required ]
    });
    editorStyle = {
        height: '50px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    estaEditando = false;
    //parametricas
    estadoArray: Dominio[] = [];
    listaUsuarios: any[] = [];
    contratoPolizaActualizacionRevisionAprobacionObservacion: any[] = [];

    constructor(
        private fb: FormBuilder,
        private common: CommonService,
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService )
    {
        this.common.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria )
            .subscribe( getUsuariosByPerfil => this.listaUsuarios = getUsuariosByPerfil );
        this.common.listaEstadoRevision()
            .subscribe( listaEstadoRevision => this.estadoArray = listaEstadoRevision );
    }

    ngOnInit(): void {
        this.getRevision();
    }

    getRevision() {
        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                this.addressForm.get( 'contratoPolizaActualizacionId' ).setValue( this.contratoPolizaActualizacion.contratoPolizaActualizacionId );

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion.length > 0 ) {
                        this.contratoPolizaActualizacionRevisionAprobacionObservacion = this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion;
                        this.contadorRevision = this.contratoPolizaActualizacionRevisionAprobacionObservacion.length;

                        const revision = this.contratoPolizaActualizacionRevisionAprobacionObservacion.filter( revision => revision.estadoSegundaRevision === this.estadosRevision.aprobacion );

                        if ( revision.length > 0 ) {
                            const ultimaRevision = revision[ revision.length - 1 ];

                            if ( this.contratoPolizaActualizacionRevisionAprobacionObservacion[ this.contratoPolizaActualizacionRevisionAprobacionObservacion.length - 1 ] === ultimaRevision ) {
                                this.contadorRevision--;

                                this.addressForm.setValue(
                                    {
                                        contratoPolizaActualizacionId: ultimaRevision.contratoPolizaActualizacionId,
                                        contratoPolizaActualizacionRevisionAprobacionObservacionId: ultimaRevision.contratoPolizaActualizacionRevisionAprobacionObservacionId,
                                        fechaRevision: ultimaRevision.segundaFechaRevision !== undefined ? new Date( ultimaRevision.segundaFechaRevision ) : null,
                                        estadoRevision: ultimaRevision.estadoSegundaRevision !== undefined ? ultimaRevision.estadoSegundaRevision : null,
                                        fechaAprob: ultimaRevision.fechaAprobacion !== undefined ? ultimaRevision.fechaAprobacion : null,
                                        responsableAprob: ultimaRevision.responsableAprobacionId !== undefined ? ultimaRevision.responsableAprobacionId : null,
                                        observacionesGenerales: ultimaRevision.observacionGeneral !== undefined ? ultimaRevision.observacionGeneral : null
                                    }
                                )
                            }
                        }
                    }
                }
            }
        }
    }

    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    getEstadoRevision( codigo: string ) {
        if ( this.estadoArray.length > 0 ) {
            const estado = this.estadoArray.find( estado => estado.codigo === codigo );

            if ( estado !== undefined ) {
                return estado.nombre;
            }
        }
    }

    getResponsable( usuarioId: number ) {
        if ( this.listaUsuarios.length > 0 ) {
            const usuario = this.listaUsuarios.find( usuario => usuario.usuarioId === usuarioId );

            if ( usuario !== undefined ) {
                return `${ this.firstLetterUpperCase( usuario.primerNombre ) } ${ this.firstLetterUpperCase( usuario.primerApellido ) }`
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }
  
    maxLength( e: any, n: number ) {
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

    checkDisabledBtn() {

        if ( this.addressForm.dirty === true && this.addressForm.get( 'fechaRevision' ).value !== null && this.addressForm.get( 'estadoRevision' ).value !== null ) {
            return false;
        }

        return true;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }
  
    onSubmit() {
        this.estaEditando = true;

        if ( this.addressForm.get( 'estadoRevision' ).value === this.estadosRevision.devuelta ) {
            this.addressForm.get( 'contratoPolizaActualizacionRevisionAprobacionObservacionId' ).setValue( 0 );
        }

        const contratoPolizaActualizacionRevisionAprobacionObservacion = () => {
            return [
                {
                    contratoPolizaActualizacionRevisionAprobacionObservacionId: this.addressForm.get( 'contratoPolizaActualizacionRevisionAprobacionObservacionId' ).value,
                    contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
                    segundaFechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : null,
                    estadoSegundaRevision: this.addressForm.get( 'estadoRevision' ).value,
                    fechaAprobacion: this.addressForm.get( 'fechaAprob' ).value !== null ? new Date( this.addressForm.get( 'fechaAprob' ).value ).toISOString() : null,
                    responsableAprobacionId: this.addressForm.get( 'responsableAprob' ).value,
                    observacionGeneral: this.addressForm.get( 'observacionesGenerales' ).value
                }
            ]
        }

        this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion = contratoPolizaActualizacionRevisionAprobacionObservacion();

        this.actualizarPolizaSvc.createorUpdateCofinancing( this.contratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarActualizacionesPolizasYGarantias/verDetalleEditarPoliza', this.contratoPoliza.contratoPolizaId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
