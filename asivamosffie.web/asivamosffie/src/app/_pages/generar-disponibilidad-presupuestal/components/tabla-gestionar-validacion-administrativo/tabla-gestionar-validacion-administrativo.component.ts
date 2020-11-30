import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface PeriodicElement {
  id: number;
  llaveMen: string;  
  nombreAportante: string;
  valorAportante: number;
  //estado: boolean;  
  ver:boolean;
}


@Component({
  selector: 'app-tabla-gestionar-validacion-administrativo',
  templateUrl: './tabla-gestionar-validacion-administrativo.component.html',
  styleUrls: ['./tabla-gestionar-validacion-administrativo.component.scss']
})
export class TablaGestionarValidacionAdministrativoComponent implements OnInit {

  displayedColumns: string[] = [
    'llaveMen',    
    'nombreAportante',
    'valorAportante',
    //'estado',
    //'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(public dialog: MatDialog) { }

  @Input()proyectos: any;
  @Input()codigo: any;
  @Input()ver: any;
  
  ngOnInit(): void {
    console.log(this.ver);
    let elements:PeriodicElement[]=[];
    this.proyectos.forEach(element => {
      elements.push({
        llaveMen:element.llaveMen,
        //estado:element.valorGestionado>0,//
        id:element.aportanteID,//el aprotante id        
        nombreAportante:element.nombreAportante,
        valorAportante:element.valorAportante,
        ver:this.ver
      });  
    });
    console.log(elements);
    this.dataSource = new MatTableDataSource(elements);
    this.inicializarTabla();
  }
  inicializarTabla() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  } 

}
