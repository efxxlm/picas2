import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { Component, ElementRef, Input, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-inicio',
  templateUrl: './tabla-fase-inicio.component.html',
  styleUrls: ['./tabla-fase-inicio.component.scss']
})
export class TablaFaseInicioComponent implements OnInit {


    @Input() esRegistroNuevo = true;
    @Input() formFaseInicio: FormGroup;
    @Input() listaFaseInicio: any[];
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

        this.listaFaseInicio.forEach( registro => {
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

    validateMainColor( registro: any ) {
        if ( !( registro.get('crear').invalid && registro.get('crear').touched || registro.get('crear').invalid ) ) {
            return true;
        }
    }

}
