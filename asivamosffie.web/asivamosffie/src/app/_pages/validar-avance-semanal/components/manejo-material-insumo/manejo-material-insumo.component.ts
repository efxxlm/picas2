import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-manejo-material-insumo',
  templateUrl: './manejo-material-insumo.component.html',
  styleUrls: ['./manejo-material-insumo.component.scss']
})
export class ManejoMaterialInsumoComponent implements OnInit {

    @Input() formManejoMaterialInsumo: FormGroup;
    @Input() materialInsumo: any;
    @Input() esVerDetalle = false;
    manejoMaterialInsumo: any;
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

    get proveedores() {
        return this.formManejoMaterialInsumo.get( 'proveedores' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog ) { }

    ngOnInit(): void {
        if ( this.materialInsumo !== undefined && this.materialInsumo.length > 0 ) {
            this.manejoMaterialInsumo = this.materialInsumo[0].manejoMaterialesInsumo;
            this.proveedores.clear();
            if ( this.manejoMaterialInsumo === undefined ) {
                this.proveedores.push(
                    this.fb.group({
                        proveedor: [ '' ],
                        requierePermisosAmbientalesMineros: [ null ],
                        urlRegistroFotografico: [ '' ],
                        manejoMaterialesInsumosProveedorId: [ 0 ],
                        manejoMaterialesInsumosId: [ 0 ]
                    })
                );
            } else {
                if ( this.manejoMaterialInsumo.manejoMaterialesInsumosProveedor.length === 0 ) {
                    this.proveedores.push(
                        this.fb.group({
                            proveedor: [ '' ],
                            requierePermisosAmbientalesMineros: [ null ],
                            urlRegistroFotografico: [ '' ],
                            manejoMaterialesInsumosProveedorId: [ 0 ],
                            manejoMaterialesInsumosId: [ 0 ]
                        })
                    );
                } else {
                    for ( const proveedor of this.manejoMaterialInsumo.manejoMaterialesInsumosProveedor ) {
                        this.proveedores.push( this.fb.group(
                            {
                                proveedor: proveedor.proveedor !== undefined ? proveedor.proveedor : '',
                                requierePermisosAmbientalesMineros: proveedor.requierePermisosAmbientalesMineros !== undefined ?
                                                                    proveedor.requierePermisosAmbientalesMineros : null,
                                urlRegistroFotografico: proveedor.urlRegistroFotografico !== undefined ? proveedor.urlRegistroFotografico : '',
                                manejoMaterialesInsumosProveedorId: proveedor.manejoMaterialesInsumosProveedorId,
                                manejoMaterialesInsumosId: proveedor.manejoMaterialesInsumosId
                            }
                        ) );
                    }
                }
                this.formManejoMaterialInsumo.patchValue( {
                    manejoMaterialesInsumosId: this.manejoMaterialInsumo.manejoMaterialesInsumosId,
                    estanProtegidosDemarcadosMateriales:    this.manejoMaterialInsumo.estanProtegidosDemarcadosMateriales !== undefined
                                                            ? this.manejoMaterialInsumo.estanProtegidosDemarcadosMateriales : null,
                    requiereObservacion:    this.manejoMaterialInsumo.requiereObservacion !== undefined
                                            ? this.manejoMaterialInsumo.requiereObservacion : null,
                    observacion: this.manejoMaterialInsumo.observacion !== undefined ? this.manejoMaterialInsumo.observacion : null,
                    url: this.manejoMaterialInsumo.url !== undefined ? this.manejoMaterialInsumo.url : null
                } );
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

}
