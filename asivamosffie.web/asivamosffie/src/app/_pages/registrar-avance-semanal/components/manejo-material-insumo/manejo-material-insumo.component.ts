import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-manejo-material-insumo',
  templateUrl: './manejo-material-insumo.component.html',
  styleUrls: ['./manejo-material-insumo.component.scss']
})
export class ManejoMaterialInsumoComponent implements OnInit {

    @Input() formManejoMaterialInsumo: FormGroup;
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

    addProveedor() {
        this.proveedores.push(
            this.fb.group({
                proveedor: [ '' ],
                requierePermisosAmbientalesMineros: [ null ],
                urlRegistroFotografico: [ '' ]
            })
        );
    }

    deleteProveedor( index: number ) {
        this.proveedores.removeAt( index );
    }

}
