import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-manejo-residuos-construccion',
  templateUrl: './manejo-residuos-construccion.component.html',
  styleUrls: ['./manejo-residuos-construccion.component.scss']
})
export class ManejoResiduosConstruccionComponent implements OnInit {

    @Input() formManejoResiduosConstruccion: FormGroup;
    @Input() residuosConstruccion: any;
    @Input() esVerDetalle = false;
    manejoResiduosConstruccion: any;
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

    get gestorResiduos() {
        return this.formManejoResiduosConstruccion.get( 'manejoResiduosConstruccionDemolicionGestor' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        if ( this.residuosConstruccion !== undefined && this.residuosConstruccion.length > 0 ) {
            this.manejoResiduosConstruccion = this.residuosConstruccion[0].manejoResiduosConstruccionDemolicion;
            this.gestorResiduos.clear();
            if ( this.manejoResiduosConstruccion === undefined ) {
                this.gestorResiduos.push(
                    this.fb.group(
                        {
                            manejoResiduosConstruccionDemolicionGestorId: [ 0 ],
                            manejoResiduosConstruccionDemolicionId: [ 0 ],
                            nombreGestorResiduos: [ '' ],
                            tienePermisoAmbiental: [ null ],
                            url: [ '' ]
                        }
                    )
                );
            } else {
                for ( const gestor of this.manejoResiduosConstruccion.manejoResiduosConstruccionDemolicionGestor ) {
                    this.gestorResiduos.push(
                        this.fb.group(
                            {
                                manejoResiduosConstruccionDemolicionGestorId: gestor.manejoResiduosConstruccionDemolicionGestorId,
                                manejoResiduosConstruccionDemolicionId: gestor.manejoResiduosConstruccionDemolicionId,
                                nombreGestorResiduos: gestor.nombreGestorResiduos !== undefined ? gestor.nombreGestorResiduos : '',
                                tienePermisoAmbiental: gestor.tienePermisoAmbiental !== undefined ? gestor.tienePermisoAmbiental : null,
                                url: gestor.url !== undefined ? gestor.url : ''
                            }
                        )
                    );
                }
                this.formManejoResiduosConstruccion.patchValue(
                    {
                        manejoResiduosConstruccionDemolicionId: this.manejoResiduosConstruccion.manejoResiduosConstruccionDemolicionId,
                        estaCuantificadoRCD:    this.manejoResiduosConstruccion.estaCuantificadoRcd !== undefined
                                                ? this.manejoResiduosConstruccion.estaCuantificadoRcd : null,
                        requiereObservacion:    this.manejoResiduosConstruccion.requiereObservacion !== undefined
                                                ? this.manejoResiduosConstruccion.requiereObservacion : null,
                        observacion:    this.manejoResiduosConstruccion.observacion !== undefined
                                        ? this.manejoResiduosConstruccion.observacion : null,
                        seReutilizadorResiduos: this.manejoResiduosConstruccion.seReutilizadorResiduos !== undefined
                                                ? this.manejoResiduosConstruccion.seReutilizadorResiduos : null,
                        cantidadToneladas:  this.manejoResiduosConstruccion.cantidadToneladas !== undefined
                                            ? String( this.manejoResiduosConstruccion.cantidadToneladas ) : ''

                    }
                );
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
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

}
