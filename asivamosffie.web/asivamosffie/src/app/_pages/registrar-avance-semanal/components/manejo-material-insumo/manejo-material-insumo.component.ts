import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-manejo-material-insumo',
  templateUrl: './manejo-material-insumo.component.html',
  styleUrls: ['./manejo-material-insumo.component.scss']
})
export class ManejoMaterialInsumoComponent implements OnInit {

    @Input() formManejoMaterialInsumo: FormGroup;
    @Input() materialInsumo: any;
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
    booleanosActividadRelacionada: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];

    get proveedores() {
      return this.formManejoMaterialInsumo.get( 'proveedores' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder ) { }

    ngOnInit(): void {
        if ( this.materialInsumo !== undefined && this.materialInsumo.length > 0 ) {
            this.manejoMaterialInsumo = this.materialInsumo[0].manejoMaterialesInsumo;
            console.log( this.manejoMaterialInsumo );
            const manejoProveedor = [];
            for ( const proveedor of this.manejoMaterialInsumo.manejoMaterialesInsumosProveedor ) {
                manejoProveedor.push(
                    {
                        proveedor: proveedor.proveedor !== undefined ? proveedor.proveedor : '',
                        requierePermisosAmbientalesMineros: proveedor.requierePermisosAmbientalesMineros ?
                                                            proveedor.requierePermisosAmbientalesMineros : null,
                        urlRegistroFotografico: proveedor.urlRegistroFotografico !== undefined ? proveedor.urlRegistroFotografico : '',
                        manejoMaterialesInsumosProveedorId: proveedor.manejoMaterialesInsumosProveedorId
                    }
                );
            }
            this.formManejoMaterialInsumo.setValue( {
                manejoMaterialesInsumosId: this.manejoMaterialInsumo.manejoMaterialesInsumosId,
                proveedores: manejoProveedor,
                estanProtegidosDemarcadosMateriales:    this.manejoMaterialInsumo.estanProtegidosDemarcadosMateriales !== undefined
                                                        ? this.manejoMaterialInsumo.estanProtegidosDemarcadosMateriales : null,
                requiereObservacion:    this.manejoMaterialInsumo.requiereObservacion !== undefined
                                        ? this.manejoMaterialInsumo.requiereObservacion : null,
                observacion: this.manejoMaterialInsumo.observacion !== undefined ? this.manejoMaterialInsumo.observacion : null,
                url: this.manejoMaterialInsumo.url !== undefined ? this.manejoMaterialInsumo.url : null
            } );
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

    addProveedor() {
        this.proveedores.push(
            this.fb.group({
                proveedor: [ '' ],
                requierePermisosAmbientalesMineros: [ null ],
                urlRegistroFotografico: [ '' ],
                manejoMaterialesInsumosProveedorId: [ 0 ]
            })
        );
    }

    deleteProveedor( index: number ) {
        this.proveedores.removeAt( index );
    }

}
