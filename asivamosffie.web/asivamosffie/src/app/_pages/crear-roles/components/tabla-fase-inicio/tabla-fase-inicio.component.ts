import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-inicio',
  templateUrl: './tabla-fase-inicio.component.html',
  styleUrls: ['./tabla-fase-inicio.component.scss']
})
export class TablaFaseInicioComponent implements OnInit {

    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    constructor( private renderer: Renderer2 ) { }

    ngOnInit(): void {
        const dataTable = [
            {
                funcionalidad: 'Gestionar usuarios',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Gestionar lista de chequeo',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Crear roles',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Gestionar parametricas',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            }
        ]
        this.dataSource = new MatTableDataSource( dataTable );
        setTimeout(() => {
            this.renderer.setStyle( this.heightAside.nativeElement, 'height', `${ this.table.nativeElement.querySelector('tbody').offsetHeight }px` );
        }, 5);
    }

}
