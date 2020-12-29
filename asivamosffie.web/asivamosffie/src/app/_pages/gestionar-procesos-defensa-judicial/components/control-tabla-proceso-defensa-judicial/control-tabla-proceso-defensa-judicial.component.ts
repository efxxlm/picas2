import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-control-tabla-proceso-defensa-judicial',
  templateUrl: './control-tabla-proceso-defensa-judicial.component.html',
  styleUrls: ['./control-tabla-proceso-defensa-judicial.component.scss']
})
export class ControlTablaProcesoDefensaJudicialComponent implements OnInit {
  displayedColumns: string[] = ['fechaRegistro', 'legitimacionPasivaActiva', 'tipoAccion', 'numeroProceso', 'estadoProceso', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    
  ]
  constructor(private router: Router,
    private defensaServices:DefensaJudicialService,public dialog: MatDialog, ) { }

  ngOnInit(): void {
    this.defensaServices.GetListGrillaProcesosDefensaJudicial().subscribe(
      response=>{
        console.log(response);
        this.dataSource = new MatTableDataSource(response);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      }
    );
    
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irNuevo() {
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial']);
  }
  editProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',id]);
  }
  actualizarProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/actualizarProceso',id]);
  }
  GetPlantillaDefensaJudicial(id)
  {
    this.defensaServices.GetPlantillaDefensaJudicial(id)
    .subscribe(respuesta => {
      const documento = 'ficha.pdf';
      const text = documento,
      blob = new Blob([respuesta], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }

  eliminar(id)
  {
    this.defensaServices.EliminarDefensaJudicial(id)
    .subscribe(respuesta => {
      this.openDialog("",respuesta.message,true);
    });
  }


  openDialog(modalTitle: string, modalText: string,redirect?:boolean) {
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
