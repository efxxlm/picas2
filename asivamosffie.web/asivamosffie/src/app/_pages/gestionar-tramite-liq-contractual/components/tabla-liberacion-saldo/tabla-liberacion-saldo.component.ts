import { Component, ViewChild, OnInit, Input} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder} from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ReleaseBalanceService } from 'src/app/core/_services/releaseBalance/release-balance.service';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-tabla-liberacion-saldo',
  templateUrl: './tabla-liberacion-saldo.component.html',
  styleUrls: ['./tabla-liberacion-saldo.component.scss']
})
export class TablaLiberacionSaldoComponent implements OnInit {
  @Input() tablaAportantes: any[];
  @Input() balanceFinanciero: any;

  estaEditando: boolean;
  estadoInforme = '0';
  registroCompleto = false;
  semaforo= false;
  noGuardado=false;
  soloMostrarObservacion=false;
  ELEMENT_DATA: any[] = [];

  displayedColumns: string[] = [
    'nombreUso',
    'nombreAportante',
    'nombreFuente',
    'valorUso',
    'saldoDisponible',
    'valorLiberar'
  ];
  listAportantes : any[] = [];
  listaUsos : any[] = [];
  addressForm = this.fb.group({});
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  proyectoId = 0;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute
    ) {
      this.proyectoId = Number( this.activatedRoute.snapshot.params.id )
    }

  ngOnInit(): void {
    this.listaUsos = [];
    this.listAportantes = [];
    var result = this.groupListByUso(this.tablaAportantes,"codigoUso");
    this.tablaAportantes.forEach(r=>{
      if(!this.listaUsos.find(lu => lu.codigoUso == r.codigoUso)){
        this.listaUsos.push({
          codigoUso: r.codigoUso,
          nombreUso: r.nombreUso,
          valorSolicitud: r.valorSolicitud
        })
      }
    })
    this.listaUsos.forEach(r => {
        const element = result[r.codigoUso];
        if(element !== null && element != undefined){
          let list = [];
          element.forEach(dataAportante => {
            //this.estadoSemaforo.emit( 'en-proceso' );

              list.push({
                nombreAportante: dataAportante.nombreAportante,
                nombreFuente: dataAportante.nombreFuente,
                fuenteFinanciacionId: dataAportante.fuenteFinanciacionId,
                fuenteRecursosCodigo: dataAportante.fuenteRecursosCodigo,
                cofinanciacionAportanteId: dataAportante.cofinanciacionAportanteId,
                componenteUsoId: dataAportante.componenteUsoId,
                componenteUsoHistoricoId: dataAportante.componenteUsoHistoricoId ?? 0,
                valorUso: dataAportante.valorUso,
                saldo: dataAportante.saldoTesoral ?? 0,
                valorSolicitud: dataAportante.valorSolicitud ?? 0,
                valorLiberar : dataAportante.valorLiberar ?? null,
              });
          });
          this.listAportantes.push({
            codigoUso: r.codigoUso,
            nombreUso: r.nombreUso,
            data: list,
            valorSolicitud: r.valorSolicitud
          });
        }
    })
    this.dataSource.data = this.listAportantes;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

    groupListByUso(list, key) {
      return list.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
      }, {});
    };
}
