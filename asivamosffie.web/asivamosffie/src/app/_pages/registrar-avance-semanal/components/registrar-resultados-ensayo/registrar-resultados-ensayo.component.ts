import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-registrar-resultados-ensayo',
  templateUrl: './registrar-resultados-ensayo.component.html',
  styleUrls: ['./registrar-resultados-ensayo.component.scss']
})
export class RegistrarResultadosEnsayoComponent implements OnInit {

    formMuestra: FormGroup;
    ensayoLaboratorio: any;
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };

    get muestras() {
        return this.formMuestra.get( 'muestras' ) as FormArray;
    }

    constructor(
        private location: Location,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
        this.getEnsayoLaboratorio();
    }

    ngOnInit(): void {
    }

    getEnsayoLaboratorio() {
        this.avanceSemanalSvc.getEnsayoLaboratorioMuestras( Number( this.activatedRoute.snapshot.params.idEnsayo ) )
            .subscribe(
                response => {
                    this.ensayoLaboratorio = response;
                    console.log( this.ensayoLaboratorio );
                    if ( this.ensayoLaboratorio.ensayoLaboratorioMuestra.length > 0 ) {
                        this.muestras.clear();
                        for ( const muestra of this.ensayoLaboratorio.ensayoLaboratorioMuestra ) {
                            this.muestras.push(
                                this.fb.group(
                                    {
                                        ensayoLaboratorioMuestraId: muestra.ensayoLaboratorioMuestraId,
                                        gestionObraCalidadEnsayoLaboratorioId: muestra.gestionObraCalidadEnsayoLaboratorioId,
                                        fechaEntregaResultado:  muestra.fechaEntregaResultado !== undefined
                                                                ? new Date( muestra.fechaEntregaResultado ) : null,
                                        nombreMuestra: muestra.nombreMuestra !== undefined ? muestra.nombreMuestra : '',
                                        observacion: muestra.observacion !== undefined ? muestra.observacion : null
                                    }
                                )
                            );
                        }
                    } else {
                        this.muestras.clear();
                        for ( let i = 0; i < this.ensayoLaboratorio.numeroMuestras; i++ ) {
                            this.muestras.push(
                                this.fb.group(
                                    {
                                        ensayoLaboratorioMuestraId: 0,
                                        gestionObraCalidadEnsayoLaboratorioId: this.ensayoLaboratorio.gestionObraCalidadEnsayoLaboratorioId,
                                        fechaEntregaResultado: [null],
                                        nombreMuestra: [''],
                                        observacion: ['']
                                    }
                                )
                            );
                        }
                    }
                }
            );
    }

    getRutaAnterior() {
      this.location.back();
    }

    crearFormulario() {
        this.formMuestra = this.fb.group(
            {
                muestras: this.fb.array( [] )
            }
        );
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
        const pGestionObraCalidadEnsayoLaboratorio = this.ensayoLaboratorio;
        this.muestras.controls.forEach( value => {
            value.get( 'fechaEntregaResultado' ).setValue(
                value.get( 'fechaEntregaResultado' ).value !== null ?
                new Date( value.get( 'fechaEntregaResultado' ).value ).toISOString() : null
            );
        } );
        pGestionObraCalidadEnsayoLaboratorio.ensayoLaboratorioMuestra = this.muestras.value;
        console.log( pGestionObraCalidadEnsayoLaboratorio );
        this.avanceSemanalSvc.createEditEnsayoLaboratorioMuestra( pGestionObraCalidadEnsayoLaboratorio )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.ensayoLaboratorio = undefined;
                    this.getEnsayoLaboratorio();
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
