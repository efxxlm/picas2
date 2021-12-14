import { Component, OnInit, ViewChild, ɵConsole, AfterViewInit, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-table-control-recursos',
  templateUrl: './table-control-recursos.component.html',
  styleUrls: ['./table-control-recursos.component.scss']
})
export class TableControlRecursosComponent implements OnInit, AfterViewInit {

  @Input() esVerDetalle: boolean;
  @Input() valorComprometidoDDP: number ;
  @Input() saldoActual: number ;

  dataTable = [];
  displayedColumns: string[] = [
    'fechaCreacion',
    'nombreCuentaBanco',
    'aportanteId',
    'numeroRp',
    'vigenciaCofinanciacionId',
    'fechaConsignacion',
    'valorConsignacion',
    'controlRecursoId'
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
    this.activatedRoute.params.subscribe( param => {
      this.idFuente = param['idFuente'];
      this.dataTable = [];
      this.fuenteFinanciacionServices.getSourceFundingBySourceFunding( this.idFuente ).subscribe( listaFuentes => {
        listaFuentes.forEach(element => {
          this.dataTable.push({
            fechaCreacion: element.fechaCreacion,
            nombreCuentaBanco: element.cuentaBancaria.nombreCuentaBanco,
            numeroCuentaBanco: element.cuentaBancaria.numeroCuentaBanco,
            aportanteId: element.fuenteFinanciacion.aportanteId,
            numeroRp: element.registroPresupuestal ? element.registroPresupuestal.numeroRp : 'No aplica',
            vigenciaCofinanciacionId: element.vigenciaAporte?.vigenciaAporte,
            fechaConsignacion: element.fechaConsignacion,
            valorConsignacion: element.valorConsignacion,
            controlRecursoId: element.controlRecursoId
          })
        });
        let valorConsignacion = 0;
        this.dataTable.forEach((element,index) => {
          if(element.valorConsignacion > 0){
            valorConsignacion += element.valorConsignacion;
          }
          element.fechaCreacion = element.fechaCreacion
            ? element.fechaCreacion.split('T')[0].split('-').reverse().join('/')
            : '';
          element.fechaConsignacion = element.fechaConsignacion
            ? element.fechaConsignacion.split('T')[0].split('-').reverse().join('/')
            : '';
            //asignarle al útimo
            if (index === this.dataTable.length - 1){
              if(this.valorComprometidoDDP < valorConsignacion){
                element.lastOne = true;
              }
            }else{
              element.lastOne = false;
            }
        });
        console.log(this.dataTable);
        this.dataSource = new MatTableDataSource(this.dataTable);
        this.ngAfterViewInit()
      })
    });


  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
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
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  openDialogSiNo(modalTitle: string, modalText: string,borrarForm: any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });
    dialogRef.afterClosed().subscribe(result => {
      // console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.fuenteFinanciacionServices.DeleteResourceFundingBySourceFunding(borrarForm).subscribe(res=>{
          // console.log(res);
          this.openDialog("",res.message);
        });
      }
    });
  }
  openDialog(modalTitle: string, modalText: string,borrarForm?: any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:false }
    });
    dialogRef.afterClosed().subscribe(result => {
      // console.log(`Dialog result: ${result}`);

        this.router.navigate(['/gestionarFuentes/controlRecursos', this.idFuente, 0])
        setTimeout(() => {
          location.reload();
        }, 1000);

    });
  }


  editar(e: number) {
    this.router.navigate(['/gestionarFuentes/controlRecursos', this.idFuente, e])
    // console.log(e);
  }


  verDetalle(e: number) {
    this.router.navigate(['/gestionarFuentes/verDetalleControlRecursos', this.idFuente, e])
    // console.log(e);
  }

  eliminar(e: any) {
    console.log(this.valorComprometidoDDP);
    // console.log(e);
    if(this.saldoActual < e.valorConsignacion){
      this.openDialog('','<b>No se puede eliminar la consignación, supera el valor comprometido.</b>');
      return;
    }
    this.openDialogSiNo("","<b>¿Está seguro de eliminar este registro?</b>",e.controlRecursoId);
  }

}
