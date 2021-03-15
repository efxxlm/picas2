import { Component, ElementRef, Input, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-cierre',
  templateUrl: './tabla-fase-cierre.component.html',
  styleUrls: ['./tabla-fase-cierre.component.scss']
})
export class TablaFaseCierreComponent implements OnInit {

    @Input() esRegistroNuevo = true;
    @Input() formFaseInicio: FormGroup;
    @Input() listaFaseCierre: any[];
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    get registros() {
        return this.formFaseInicio.get( 'registros' ) as FormArray;
    }

    constructor(
        private renderer: Renderer2,
        private fb: FormBuilder )
    {
    }

    ngOnInit(): void {

        this.listaFaseCierre.forEach( registro => {
            this.registros.push( this.fb.group(
                {
                    menuId: [ registro.menuId, Validators.required ],
                    funcionalidad: [ registro.nombre, Validators.required ],
                    crear: [ null, Validators.required ],
                    modificar: [ null, Validators.required ],
                    consultar: [ null, Validators.required ],
                    eliminar: [ null, Validators.required ],
                }
            ) )
        } );

        this.dataSource = new MatTableDataSource( this.registros.controls );
        setTimeout(() => {
            this.renderer.setStyle( this.heightAside.nativeElement, 'height', `${ this.table.nativeElement.querySelector('tbody').offsetHeight }px` );
        }, 5);
    }

}
