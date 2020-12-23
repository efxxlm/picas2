import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-actualizar-reclamacion-aseg-cc',
  templateUrl: './actualizar-reclamacion-aseg-cc.component.html',
  styleUrls: ['./actualizar-reclamacion-aseg-cc.component.scss']
})
export class ActualizarReclamacionAsegCcComponent implements OnInit {
  idControversia: any;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  tipoControversia: string;
  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
  }
  loadService(){

  }
}
