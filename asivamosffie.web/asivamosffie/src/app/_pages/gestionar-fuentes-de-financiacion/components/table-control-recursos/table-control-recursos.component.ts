import { Component, OnInit, ViewChild, ɵConsole } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface ControlRecursos {
  id: number;
  fechaCreacion: string;
  nombreCuenta: string;
  rp: string;
  vigencia: number;
  fechaConsignacion: string;
  valorConsignacion: number;
}



// const ELEMENT_DATA: ControlRecursos[] = [
//   {
//     id: 0,
//     fechaCreacion: '20/05/2020',
//     nombreCuenta: 'Recursos corrientes ',
//     rp: 'CAE58733398554',
//     vigencia: 2021,
//     fechaConsignacion: '24/06/2020',
//     valorConsignacion: 33000000,
//   },
// ];


@Component({
  selector: 'app-table-control-recursos',
  templateUrl: './table-control-recursos.component.html',
  styleUrls: ['./table-control-recursos.component.scss']
})
export class TableControlRecursosComponent implements OnInit {

  displayedColumns: string[] = [
    'fechaCreacion',
    'nombreCuenta',
    'rp',
    'vigencia',
    'fechaConsignacion',
    'valorConsignacion',
    'id'
  ];

  idFuente: number = 0;

  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private activatedRoute: ActivatedRoute,
                private fuenteFinanciacionServices: FuenteFinanciacionService,
                private router: Router,
                private dialog: MatDialog,
             ) 
  { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';

    this.activatedRoute.params.subscribe( param => {
      this.idFuente = param['idFuente'];
      this.fuenteFinanciacionServices.getSourceFundingBySourceFunding( this.idFuente ).subscribe( listaFuentes => {
        this.dataSource = new MatTableDataSource(listaFuentes);
        console.log(listaFuentes);
      })
    });

    
  }
  openDialogSiNo(modalTitle: string, modalText: string,borrarForm: any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.fuenteFinanciacionServices.DeleteResourceFundingBySourceFunding(borrarForm).subscribe(res=>{
          console.log(res);
        });
      }           
    });
  }


  editar(e: number) {
    this.router.navigate(['/gestionarFuentes/controlRecursos', this.idFuente, e])
    console.log(e);
  }

  eliminar(e: number) {
    console.log(e);
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",e);
  }

}
