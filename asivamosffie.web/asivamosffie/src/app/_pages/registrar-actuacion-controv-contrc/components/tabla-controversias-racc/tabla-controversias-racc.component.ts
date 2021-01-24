import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-controversias-racc',
  templateUrl: './tabla-controversias-racc.component.html',
  styleUrls: ['./tabla-controversias-racc.component.scss']
})
export class TablaControversiasRaccComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'numeroSolicitud',
    'numeroContrato',
    'tipoControversia',
    'actuacion',
    'fechaActuacion',
    'estadoActuacion',
    'gestion'
  ];
  dataTable: any[] = [
  ]
  constructor(private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,) { }

  ngOnInit(): void {
    this.conServices.GetListGrillaControversiaActuaciones().subscribe(
      result=>{
        this.dataSource = new MatTableDataSource(result);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      }
    );
    
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  descargarControversia(id){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/actualizarTramite', id]);
  }

  irActualizarTramite(id){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/actualizarTramite', id]);
  }

  finalizarActuacion(id){
    this.conServices.FinalizarActuacion(id).subscribe(
      result=>
      {
        this.openDialog("",result.message,true);
      }
    );
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
