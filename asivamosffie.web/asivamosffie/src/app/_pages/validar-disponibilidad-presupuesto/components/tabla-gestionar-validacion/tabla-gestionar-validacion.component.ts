import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FormGestionarFuentesComponent } from '../form-gestionar-fuentes/form-gestionar-fuentes.component';

export interface PeriodicElement {
  id: number;
  llaveMen: string;
  tipoInterventor: string;
  departamento: string;
  municipio: string;
  institucion: string;
  sede: string;
  nombreAportante: string;
  valorAportante: number;
  estado: boolean;
  disponibilidadPresupuestalProyectoid:number;
  valorGestionado:number;
  ver:boolean;
}


@Component({
  selector: 'app-tabla-gestionar-validacion',
  templateUrl: './tabla-gestionar-validacion.component.html',
  styleUrls: ['./tabla-gestionar-validacion.component.scss']
})
export class TablaGestionarValidacionComponent implements OnInit {

  displayedColumns: string[] = [
    'llaveMen',
    'tipoInterventor',
    'departamento',
    'institucion',
    'nombreAportante',
    'valorAportante',
    'estado',
    'id'
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
        departamento:element.departamento,
        estado:element.valorGestionado>0,//
        id:element.aportanteID,//el aprotante id
        institucion:element.institucionEducativa,
        municipio:element.municipio,
        sede:element.sede,
        nombreAportante:element.nombreAportante,
        tipoInterventor:element.tipoIntervencion,//revisar
        valorAportante:element.valorAportante,
        disponibilidadPresupuestalProyectoid:element.disponibilidadPresupuestalProyecto,
        valorGestionado:element.valorGestionado,
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

  gestionarFuentes(id: any) {
    console.log(id);
    // this.openDialog('', `El saldo actual de la fuente <b>Recursos propios</b> es menor
    // al valor solicitado de la fuente, verifique por favor.`);
    this.dialog.open(FormGestionarFuentesComponent, {
      width: '70em', data: { elemento: id, codigo:this.codigo,ver:this.ver }
    });
  }

}
