import { Component, OnInit, ViewChild,OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-actualizar-tramite-racc',
  templateUrl: './actualizar-tramite-racc.component.html',
  styleUrls: ['./actualizar-tramite-racc.component.scss']
})
export class ActualizarTramiteRaccComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  dataTable: any[] = [
  ]
  actuacionid: any;
  actuacion: any;
  noGuardado: boolean=true;
  constructor(private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,)
     { }

    
  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.actuacionid = param['id'];
      this.conServices.GetControversiaActuacionById(this.actuacionid).subscribe(
        response=>{
          this.actuacion=response;
          this.dataSource = new MatTableDataSource(response.seguimientoActuacionDerivada);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        }
      );
    });

    
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irARegistro(){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/registrarActuacionDerivada',this.actuacionid,0]);
  }
  verDetalleEditarActuacionDerivada(id){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/registrarActuacionDerivada',this.actuacionid,id]);
  }
  finalizarActuacionDerivada(id){
    this.conServices.FinalizarActuacionDerivada(id).subscribe(
      response=>{
        this.openDialog("",response.message,true);
      });

  }

  eliminarActuacionDerivada(id){
    this.openDialogSiNo("","<b>¿Esta seguro de eliminar este registro?</b>",id);
  }
  openDialogSiNo(modalTitle: string, modalText: string , id:any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.eliminarActuacionDerivadaSi(id)
      }           
    });
  }
  eliminarActuacionDerivadaSi(id){
    this.conServices.EliminarActuacionDerivada(id).subscribe(
      response=>{
        this.openDialog("","<b>La información se ha eliminado correctamente</b>",true);
      });
  }
  verDetalleActuacionDerivada(id){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/verDetalleActuacionDerivada',this.actuacionid,id]);
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        location.reload();
      });
    }
  }
}
