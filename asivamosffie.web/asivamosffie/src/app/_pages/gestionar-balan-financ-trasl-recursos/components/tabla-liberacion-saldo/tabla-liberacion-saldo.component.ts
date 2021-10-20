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
  @Input() esVerDetalle: boolean;

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
    private releaseBalanceService: ReleaseBalanceService,
    private routes: Router,
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
                esNovedad : dataAportante.esNovedad ?? false,
                novedadContractualRegistroPresupuestalId: dataAportante.novedadContractualRegistroPresupuestalId ?? false,
                componenteUsoNovedadId: dataAportante.componenteUsoNovedadId,
                componenteUsoNovedadHistoricoId: dataAportante.componenteUsoNovedadHistoricoId ?? 0,
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

  onSubmit() {
    this.estaEditando = true;
    this.noGuardado=false;
    const dataHistorico = [];
    this.dataSource.data.forEach(r =>{
      r.data.forEach(u => {
          if(u.valorLiberar != null || u.componenteUsoHistoricoId > 0 || u.componenteUsoNovedadHistoricoId > 0 ){
            dataHistorico.push({
              componenteUsoId: u.componenteUsoId,
              valorLiberar: u.valorLiberar,
              esNovedad: u.esNovedad,
              componenteUsoNovedadId: u.componenteUsoNovedadId,
              componenteUsoHistoricoId: u.componenteUsoHistoricoId,
              componenteUsoNovedadHistoricoId:  u.componenteUsoNovedadHistoricoId,
              saldo: u.saldo
            });
          }
      });
    });
    const VUsosHistorico ={
      usosHistorico:dataHistorico,
      balanceFinancieroId: this.balanceFinanciero?.balanceFinancieroId ?? 0,
      proyectoId: this.proyectoId
    }

    this.createEditHistoricalReleaseBalance(VUsosHistorico);
  }

  createEditHistoricalReleaseBalance(pUsosHistorico: any) {
    this.releaseBalanceService.createEditHistoricalReleaseBalance(pUsosHistorico)
      .subscribe(
        (respuesta: Respuesta) => {
          this.openDialog('', respuesta.message);
          this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () => this.routes.navigate(
                [
                    '/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', this.proyectoId
                ]
            )
          );
          return;
        },
        err => {
          this.openDialog('', err.message);
          return;
        }
      );
  }


    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    maxLength(e: any, n: number) {
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n, e.editor.getLength());
      }
    }

    groupListByUso(list, key) {
      return list.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
      }, {});
    };

    validateSaldo(el: any, valorSolicitud: number,codigoUso: number){
      this.dialog.closeAll();
      let valorTotalLiberar = 0;
      let index = this.dataSource.data.findIndex(d => d.codigoUso == codigoUso);
      let positionDta = -1;
      if(index != -1){
        positionDta = this.dataSource.data[index].data.findIndex(d => d.componenteUsoId == el.componenteUsoId);
      }
      if(el.valorLiberar > el.saldo){
        if(positionDta != -1)
          this.dataSource.data[index].data[positionDta].valorLiberar = null;
        this.openDialog(
          '',
          '<b>El valor a liberar no puede ser mayor al saldo disponible.</b>'
        );
        return false;
      }
      this.dataSource.data.forEach(r=>{
        r.data.forEach(d => {
          valorTotalLiberar += (d.valorUso - d.valorLiberar)
        });
      });

      if (valorTotalLiberar > valorSolicitud) {
        if(positionDta != -1)
          this.dataSource.data[index].data[positionDta].valorLiberar = null;
        this.openDialog(
          '',
          '<b>Los valores registrados superan el valor total del RP;es necesario revisar y ajustar estos valores .</b>'
        );
        return false;
      }

    }

}
