import { Component, Input, OnInit } from '@angular/core';
import { ReleaseBalanceService } from 'src/app/core/_services/releaseBalance/release-balance.service';

@Component({
  selector: 'app-liberacion-saldo',
  templateUrl: './liberacion-saldo.component.html',
  styleUrls: ['./liberacion-saldo.component.scss']
})
export class LiberacionSaldoComponent implements OnInit {

  @Input() id: number;
  @Input() esVerDetalle: boolean;
  estadoInforme = 0;
  drps: any[] = [];

  constructor(
    private releaseBalanceService: ReleaseBalanceService,
  ) { }

  ngOnInit(): void {
    this.getDrpByProyectoId();
  }

  getDrpByProyectoId() {
    this.releaseBalanceService.getDrpByProyectoId(this.id).subscribe(data => {
        this.drps = data;
        if(this.drps.length > 0){
          this.drps.forEach(r =>{
            r.estadoSemaforo = 'sin-diligenciar';
            r.registroCompleto = false;
            const total_uso = r.aportantesGrid.length;
            const total_completo_uso = r.aportantesGrid.filter(r => r.registroCompleto == 1).length;
            if(total_uso > 0){
              if(total_uso == total_completo_uso){
                r.registroCompleto = true;
                r.estadoSemaforo = 'completo';
              }else if(total_completo_uso > 0 ){
                r.estadoSemaforo = 'en-proceso';
              }
            }

          });
        }
    });
  }

}

